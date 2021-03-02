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
    [SerializeField] GameObject boardModel;
    [SerializeField] Material ocuppiedBoxMat;
    [SerializeField] Material freeBoxMat;
    [SerializeField] Camera cameraBlacks;
    [SerializeField] Camera cameraWhites;


    bool isMyTurn;
    Board chessBoard;
    Vector3 scale;
    private List<GameObject> markers;
    private List<Piece> eatenPieces;
    public Board ChessBoard { get => chessBoard; }
    public bool IsMyTurn { get => isMyTurn; }

    // Start is called before the first frame update
    void Start()
    {
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
        GetCellFromCoordinates(piece.transform.position, out row, out column);
        Box selected = chessBoard.GetBoxInternal(row, column);
        if (selected.GetPiece().CheckPossibleMovements().Contains(GetBoxFromCoordinates(targetLocation)))
        {
            Box originBox = GetBoxFromCoordinates(piece.transform.position);
            Box targetBox = GetBoxFromCoordinates(targetLocation);
            if (originBox.GetPiece().Type == Pieces.Pawn)
                ((Pawn)originBox.GetPiece()).AckFirstMovement();

            //move happens

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
            NetworkHandler.instance.SendMovement(originBox.ChessColumnCoord,originBox.ChessRowCoord,targetBox.ChessColumnCoord,targetBox.ChessRowCoord);
            isMyTurn = false;
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
            ((Pawn)originBox.GetPiece()).AckFirstMovement();


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
