using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Chess
{
    class King : Piece
    {
        private bool isFirstMovement = true;
        public bool IsFirstMovement { get => isFirstMovement; }

        public King(ChessColors color, GameObject pieceObject)
        {
            this.pieceObject = pieceObject;
            type = Pieces.King;
            this.color = color;
            isFirstMovement = true;
        }

        public override List<Box> CheckPossibleMovements()
        {
            List<Box> positions = new List<Box>();
            positions = AddVerticalPositions(positions);
            positions = AddHorizontalPositions(positions);
            positions = AddObliquePositions(positions);
            if (isFirstMovement)
                positions = AddCastles(positions);
            return positions;
        }

        public override void Move(Box destination)
        {
            throw new NotImplementedException();
        }


        private List<Box> AddVerticalPositions(List<Box> positions)
        {
            if (box.Row + 1 < 8)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(box.Row +1, box.Column).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row +1, box.Column));
                }
                
            }

            if (box.Row - 1 >= 0)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(box.Row -1, box.Column).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row -1, box.Column));
                }

            }

            return positions;
        }



        private List<Box> AddHorizontalPositions(List<Box> positions)
        {
            if ( box.Column + 1 < 8)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column + 1).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column + 1));
                }
         
            }


            if (box.Column - 1 >= 0)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column - 1).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column - 1));
                    
                }

            }

            return positions;
        }



        private List<Box> AddObliquePositions(List<Box> positions)
        {
            positions = AddTopLeft(positions);
            positions = AddTopRight(positions);
            positions = AddBottomLeft(positions);
            positions = AddBottomRight(positions);
            return positions;
        }

        private List<Box> AddTopLeft(List<Box> positions)
        {
            int row = box.Row + 1;
            int col = box.Column - 1;

            if (row < 8 && col >= 0)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(row, col).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(row, col));
                    
                }
            }

            return positions;
        }

        private List<Box> AddTopRight(List<Box> positions)
        {
            int row = box.Row + 1;
            int col = box.Column + 1;

            if (row < 8 && col < 8)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(row, col).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(row, col));

                }
            }

            return positions;
        }

        private List<Box> AddBottomLeft(List<Box> positions)
        {
            int row = box.Row - 1;
            int col = box.Column - 1;

            if (row >= 0 && col >= 0)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(row, col).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(row, col));

                }
            }

            return positions;
        }

        private List<Box> AddBottomRight(List<Box> positions)
        {
            int row = box.Row - 1;
            int col = box.Column + 1;

            if (row >= 0 && col < 8)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(row, col).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(row, col));

                }
            }

            return positions;
        }

        private List<Box> AddCastles(List<Box> positions)
        {
            AddCastleLeft(positions);
            AddCastleRight(positions);
            return positions;
        }

        private List<Box> AddCastleLeft(List<Box> positions)
        {
           
            Box boxLeft = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, 0);
            if (boxLeft.GetPiece() != null && boxLeft.GetPiece() is Rook && ((Rook)boxLeft.GetPiece()).IsFirstMovement)
            {
                for (int i = box.Column - 1; i > boxLeft.Column; i--)
                {
                    if (BoardController.instance.ChessBoard.GetBoxInternal(box.Row, i).GetPiece() != null)
                        return positions;
                }
                positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row,box.Column -2));
            }
            return positions;
        }
        private List<Box> AddCastleRight(List<Box> positions)
        {
            Box boxRight = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, 7);
            if (boxRight.GetPiece() != null && boxRight.GetPiece() is Rook && ((Rook)boxRight.GetPiece()).IsFirstMovement)
            {
                for (int i = box.Column + 1; i < boxRight.Column; i++)
                {
                    if (BoardController.instance.ChessBoard.GetBoxInternal(box.Row, i).GetPiece() != null)
                        return positions;
                }
                positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column + 2));
            }
            return positions;
        }

        public void AckFirstMovement()
        {
            isFirstMovement = false;

        }
    }
}
