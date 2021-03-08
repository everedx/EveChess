using Assets.Scripts;
using Assets.Scripts.Chess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Core.Utils;

public class BoardController : Singleton<BoardController>
{
    [SerializeField] PieceInfo[] chessPieces;
    [SerializeField] List<PieceInfo> chessPiecesPrefabs;
    [SerializeField] GameObject boardModel;
    [SerializeField] Material ocuppiedBoxMat;
    [SerializeField] Material freeBoxMat;
    [SerializeField] Camera cameraBlacks;
    [SerializeField] Camera cameraWhites;
    [SerializeField] GameObject eatenPiecesMenu;
    public bool byPassTurns;

    private bool needToChoosePiece;
    char orX, orY, taX, taY;
    bool gameOver;
    bool isMyTurn;
    Board chessBoard;
    Vector3 scale;
    private List<GameObject> markers;
    private List<Piece> eatenPieces;
    public Board ChessBoard { get => chessBoard; }
    public bool IsMyTurn { get => isMyTurn; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public List<Piece> EatenPieces { get => eatenPieces; }

    // Start is called before the first frame update
    void Start()
    {
        needToChoosePiece = false;
        gameOver = false;
        if (NetworkHandler.instance.MyColor == ChessColors.Black)
        {
            isMyTurn = false;
            cameraWhites.gameObject.SetActive(false);
            cameraBlacks.gameObject.SetActive(true);
        }
        else 
        {
            isMyTurn = true;
        }

        markers = new List<GameObject>();
        eatenPieces = new List<Piece>();
        scale = boardModel.transform.localScale;

        chessBoard = new Board();

        for (char c = 'A'; c < 'I'; c++)
            for (int i = 1; i < 9; i++)
            {
                PieceInfo info = chessPieces.ToList().Find(x => x.row == i - 1 && x.column == c - 'A');
                if (info != null)
                {
                    Box box = chessBoard.GetBoxChessCoordinates(c,i);
                    chessBoard.AddPiece(box.Row,box.Column,PieceFactory.CreatePiece(info.type,info.color, info.pieceObject));
                }
            }

            
        
    }



    public void SelectPiece(GameObject pieceObject)
    {

        foreach (GameObject go in markers)
            Destroy(go);
        markers.Clear();
        if (pieceObject != null)
        {
            int row, column;
            GetCellFromCoordinates(pieceObject.transform.position, out row, out column);
            Box selected = chessBoard.GetBoxInternal(row, column);

            foreach (Box box in selected.GetPiece().CheckPossibleMovements())
            {
                if (box.GetPiece() != null)
                {
                    CreateMesh(GetCoordinatesFromBox(box), ocuppiedBoxMat);
                }
                else
                {
                    CreateMesh(GetCoordinatesFromBox(box), freeBoxMat);
                }
            }
        }

    }


    private void GetCellFromCoordinates(Vector3 position, out int row, out int column)
    {
        row = Mathf.FloorToInt(position.z - (boardModel.transform.position.z -(0.5f*scale.z)));
        column = Mathf.FloorToInt(position.x - (boardModel.transform.position.x - (0.5f*scale.x)));
        row = Mathf.FloorToInt(row / scale.z);
        column = Mathf.FloorToInt(column / scale.x);

    }

    public void MovePiece(GameObject piece, Vector3 targetLocation)
    {
        int row, column;
        needToChoosePiece = false;
        GetCellFromCoordinates(piece.transform.position, out row, out column);
        Box selected = chessBoard.GetBoxInternal(row, column);
        if (selected.GetPiece().CheckPossibleMovements().Contains(GetBoxFromCoordinates(targetLocation)))
        {
            Box originBox = GetBoxFromCoordinates(piece.transform.position);
            Box targetBox = GetBoxFromCoordinates(targetLocation);
            if (originBox.GetPiece().Type == Pieces.Pawn)
            {
                Pawn pawn = (Pawn)originBox.GetPiece();
                if (pawn.IsFirstMovement)
                {
                    if (Mathf.Abs(targetBox.Row - originBox.Row) > 1)
                    {
                        //check en passant
                        Box leftBox = null;
                        Box rightBox = null;
                        if (targetBox.Column > 0) leftBox = chessBoard.GetBoxInternal(targetBox.Row, targetBox.Column - 1);
                        if (targetBox.Column < 7) rightBox = chessBoard.GetBoxInternal(targetBox.Row, targetBox.Column + 1);
                        if (leftBox != null)
                        {
                            Piece leftBoxPiece = leftBox.GetPiece();
                            if (leftBoxPiece != null && leftBoxPiece is Pawn) ((Pawn)leftBoxPiece).SetEnpassantPawn(pawn);
                        }
                        if (rightBox != null)
                        {
                            Piece rightBoxPiece = rightBox.GetPiece();
                            if (rightBoxPiece != null && rightBoxPiece is Pawn) ((Pawn)rightBoxPiece).SetEnpassantPawn(pawn);
                        }

                    }
                    pawn.AckFirstMovement();
                }
                else if (targetBox.Row == 0 || targetBox.Row == 7)
                {
                    if (eatenPieces.FindAll(x => x.Color == originBox.GetPiece().Color && (x is Queen || x is Bishop || x is Rook || x is Knight)).Count > 0)
                    {
                        eatenPiecesMenu.SetActive(true);
                        needToChoosePiece = true;
                    }
                    
                }
            }
            else if(originBox.GetPiece().Type == Pieces.King) 
            {
                King king = (King)originBox.GetPiece();
                king.AckFirstMovement();

               

            }
            else if (originBox.GetPiece().Type == Pieces.Rook)
            {
                Rook rook = (Rook)originBox.GetPiece();
                rook.AckFirstMovement();



            }

            //move happens

            // visuals
            if (targetBox.GetPiece() != null)
            {
                //delete gameobject
                eatenPieces.Add(targetBox.GetPiece());
                if (targetBox.GetPiece() is King)
                {
                  
                    NetworkHandler.instance.SendWinner(NetworkHandler.instance.MyColor);
                }
                GameObject go = targetBox.GetPiece().PieceObject;
                Destroy(go);
            }
            else 
            {
                Piece originPiece = originBox.GetPiece();
                if (originPiece is Pawn && ((Pawn)originPiece).EnPassantPawn != null)
                {//En passant move
                    int offset = originBox.GetPiece().Color == ChessColors.White ? -1 : 1;
                    Box enPassantEatenBox = chessBoard.GetBoxInternal(targetBox.Row + offset, targetBox.Column);
                    eatenPieces.Add(enPassantEatenBox.GetPiece());
                    NetworkHandler.instance.SendEatenPiece(enPassantEatenBox.ChessColumnCoord, enPassantEatenBox.ChessRowCoord);
                    GameObject go = enPassantEatenBox.GetPiece().PieceObject;
                    Destroy(go);
                }

                if (originPiece is King)
                {
                    if (Mathf.Abs(targetBox.Column - originBox.Column) > 1)
                    {
                        //CASTLE
                        if (targetBox.Column > originBox.Column) //right
                        {
                            Box boxOriginRook = chessBoard.GetBoxInternal(targetBox.Row, 7);
                            Box boxTargetRook = chessBoard.GetBoxInternal(targetBox.Row, targetBox.Column - 1);
                            Rook rook = (Rook)boxOriginRook.GetPiece();
                            boxTargetRook.SetPiece(boxOriginRook.GetPiece());
                            boxOriginRook.SetPiece(null);


                            rook.PieceObject.transform.position = GetCoordinatesFromBox(boxTargetRook);
                            NetworkHandler.instance.SendMovement(boxOriginRook.ChessColumnCoord, boxOriginRook.ChessRowCoord, boxTargetRook.ChessColumnCoord, boxTargetRook.ChessRowCoord);
                        }
                        else //left
                        {
                            Box boxOriginRook = chessBoard.GetBoxInternal(targetBox.Row, 0);
                            Box boxTargetRook = chessBoard.GetBoxInternal(targetBox.Row, targetBox.Column + 1);
                            Rook rook = (Rook)boxOriginRook.GetPiece();
                            boxTargetRook.SetPiece(boxOriginRook.GetPiece());
                            boxOriginRook.SetPiece(null);


                            rook.PieceObject.transform.position = GetCoordinatesFromBox(boxTargetRook);
                            NetworkHandler.instance.SendMovement(boxOriginRook.ChessColumnCoord, boxOriginRook.ChessRowCoord, boxTargetRook.ChessColumnCoord, boxTargetRook.ChessRowCoord);
                        }
                    }
                }

            }



            piece.transform.position = GetCoordinatesFromBox(targetBox);  
           

            //logic
            targetBox.SetPiece(originBox.GetPiece());
            originBox.SetPiece(null);


            if (!needToChoosePiece)
            {
                //send to the oponent
                NetworkHandler.instance.SendMovement(originBox.ChessColumnCoord, originBox.ChessRowCoord, targetBox.ChessColumnCoord, targetBox.ChessRowCoord);
                if (byPassTurns == false)
                    isMyTurn = false;

                
            }
            else 
            {
                if (byPassTurns == false)
                    isMyTurn = false;
                orX = originBox.ChessColumnCoord;
                orY = originBox.ChessRowCoord;
                taX = targetBox.ChessColumnCoord;
                taY = targetBox.ChessRowCoord;
            }
            RemoveEnPassant(targetBox.GetPiece().Color);
        }

    }

    private void RemoveEnPassant(ChessColors colorPieceMoved)
    {
        ChessColors colorToCheck=NetworkHandler.instance.MyColor; 
        if (byPassTurns)
            colorToCheck =colorPieceMoved == NetworkHandler.instance.MyColor ? NetworkHandler.instance.MyColor : NetworkHandler.instance.MyColor == ChessColors.White ? ChessColors.Black : ChessColors.White;
        foreach (Pawn p in chessBoard.GetAllPiecesOfColor<Pawn>(colorToCheck))
        {
            if(p.EnPassantPawn != null)
                 p.SetEnpassantPawn(null);
        }
    }

    public void MovePieceFromNetwork(char originColumn,char originRow,char targetColumn, char targetRow)
    {
        GameObject piece;
        Box originBox = chessBoard.GetBoxChessCoordinatesNet(originColumn, originRow);
        Debug.Log("Origin: " + originBox);
        piece = originBox.GetPiece().PieceObject;
        Debug.Log(piece);
        Box targetBox = chessBoard.GetBoxChessCoordinatesNet(targetColumn, targetRow);
        Debug.Log("Target: " + originBox);





        if (originBox.GetPiece().Type == Pieces.Pawn)
        {
            
            Pawn pawn = (Pawn)originBox.GetPiece();
            if (pawn.IsFirstMovement)
            {
                if (Mathf.Abs(targetBox.Row - originBox.Row) > 1)
                {
                    //check en passant
                    Box leftBox = null;
                    Box rightBox = null;
                    if (targetBox.Column > 0) leftBox = chessBoard.GetBoxInternal(targetBox.Row, targetBox.Column - 1);
                    if (targetBox.Column < 7) rightBox = chessBoard.GetBoxInternal(targetBox.Row, targetBox.Column + 1);
                    if (leftBox != null)
                    {
                        Piece leftBoxPiece = leftBox.GetPiece();
                        if (leftBoxPiece != null && leftBoxPiece is Pawn) ((Pawn)leftBoxPiece).SetEnpassantPawn(pawn);
                    }
                    if (rightBox != null)
                    {
                        Piece rightBoxPiece = rightBox.GetPiece();
                        if (rightBoxPiece != null && rightBoxPiece is Pawn) ((Pawn)rightBoxPiece).SetEnpassantPawn(pawn);
                    }

                }
                pawn.AckFirstMovement();
            }
            
            ((Pawn)originBox.GetPiece()).AckFirstMovement();
        }


        // visuals
        if (targetBox.GetPiece() != null)
        {
            //delete gameobject
            eatenPieces.Add(targetBox.GetPiece());
            GameObject go = targetBox.GetPiece().PieceObject;
            Destroy(go);
        }

      

        piece.transform.position = GetCoordinatesFromBox(targetBox);


        //logic
        targetBox.SetPiece(originBox.GetPiece());
        originBox.SetPiece(null);



        //send to the oponent
        isMyTurn = true;
        

    }

    public void EatPieceFromNetwork(char column, char row)
    {
        Box box = chessBoard.GetBoxChessCoordinatesNet(column, row);
        Destroy(box.GetPiece().PieceObject);
        eatenPieces.Add(box.GetPiece());
        box.SetPiece(null);

    }


    public Box GetBoxFromCoordinates(Vector3 coordinates)
    {
        int row, col;
        GetCellFromCoordinates(coordinates,out row,out col);
        return chessBoard.GetBoxInternal(row, col);
    }

    public Vector3 GetCoordinatesFromBox(Box box)
    {
        return new Vector3(boardModel.transform.position.x + box.Column * scale.x,boardModel.transform.position.y  , boardModel.transform.position.z + box.Row * scale.z);
    }


    public void ReplacePawn(Pieces piece)
    {
        Box box = chessBoard.GetBoxChessCoordinatesNet(taX,taY);
        Destroy(box.GetPiece().PieceObject);
        GameObject go = Instantiate(chessPiecesPrefabs.ToList().Find(x => x.color == NetworkHandler.instance.MyColor && x.type == piece).pieceObject);
        chessBoard.AddPiece(box.Row, box.Column, PieceFactory.CreatePiece(piece, NetworkHandler.instance.MyColor, go));
        go.transform.position = GetCoordinatesFromBox(box);
        eatenPieces.Remove(eatenPieces.Find(x => x.Color == NetworkHandler.instance.MyColor && x.Type == piece));

        //tell the other client
        NetworkHandler.instance.SendMoveAndReplace(orX, orY, taX, taY,piece);
        if (byPassTurns == false)
            isMyTurn = false;

        
    }

    public void ReplacePawnNetwork(char originColumn, char originRow, char targetColumn, char targetRow,Pieces piece)
    {
        Box originBox = chessBoard.GetBoxChessCoordinatesNet(originColumn, originRow);
        Box targetBox = chessBoard.GetBoxChessCoordinatesNet(targetColumn, targetRow);
        ChessColors color = originBox.GetPiece().Color;
        GameObject destroyable = originBox.GetPiece().PieceObject;
        Destroy(destroyable);
        originBox.SetPiece(null);

        if (targetBox.GetPiece() != null)
        {
            eatenPieces.Add(targetBox.GetPiece());
            Destroy(targetBox.GetPiece().PieceObject);
        }

        GameObject go = Instantiate(chessPiecesPrefabs.ToList().Find(x => x.color == color && x.type == piece).pieceObject);
        chessBoard.AddPiece(targetBox.Row, targetBox.Column, PieceFactory.CreatePiece(piece, color, go));
        go.transform.position = GetCoordinatesFromBox(targetBox);
        eatenPieces.Remove(eatenPieces.Find(x => x.Color == color && x.Type == piece));



        //send to the oponent
        isMyTurn = true;


    }


    private void CreateMesh(Vector3 location, Material mat)
    {
        GameObject go = new GameObject("Marker");

        go.transform.position = new Vector3(location.x - scale.x/2,location.y+0.1f,location.z - scale.z/2);
        go.transform.eulerAngles= new Vector3 (90,0,0);
        
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mat;

        MeshFilter meshFilter = go.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(scale.x, 0, 0),
            new Vector3(0, scale.z, 0),
            new Vector3(scale.x, scale.z, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
        markers.Add(go);
    }
    
}



[Serializable]
public class PieceInfo
{
    public int row, column;
    public ChessColors color;
    public Pieces type;
    public GameObject pieceObject;

}
