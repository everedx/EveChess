using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Chess
{
    class Queen : Piece
    {
        public Queen(ChessColors color, GameObject pieceObject)
        {
            type = Pieces.Queen;
            this.pieceObject = pieceObject;
            this.color = color;
        }

        public override List<Box> CheckPossibleMovements()
        {
            List<Box> positions = new List<Box>();
            positions = AddVerticalPositions(positions);
            positions = AddHorizontalPositions(positions);
            positions = AddObliquePositions(positions);

            return positions;
        }

        public override void Move(Box destination)
        {
            throw new NotImplementedException();
        }


        private List<Box> AddVerticalPositions(List<Box> positions)
        {
            for (int i = box.Row + 1; i < 8; i++)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(i, box.Column).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(i, box.Column));
                    if (piece != null)
                        break;
                }
                else
                    break;
            }


            for (int i = box.Row - 1; i >= 0; i--)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(i, box.Column).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(i, box.Column));
                    if (piece != null)
                        break;
                }
                else
                    break;
            }

            return positions;
        }



        private List<Box> AddHorizontalPositions(List<Box> positions)
        {
            for (int i = box.Column + 1; i < 8; i++)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, i).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row, i));
                    if (piece != null)
                        break;
                }
                else
                    break;
            }


            for (int i = box.Column - 1; i >= 0; i--)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, i).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row, i));
                    if (piece != null)
                        break;
                }
                else
                    break;
            }

            return positions;
        }



        private List<Box> AddObliquePositions(List<Box> positions )
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

            while (row < 8 && col >= 0)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(row, col).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(row, col));
                    if (piece != null)
                        break;
                }
                else
                    break;
                row++;
                col--;
            }

            return positions;
        }

        private List<Box> AddTopRight(List<Box> positions)
        {
            int row = box.Row + 1;
            int col = box.Column + 1;

            while (row < 8 && col < 8)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(row,col).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(row, col));
                    if (piece != null)
                        break;
                }
                else
                    break;

                row++;
                col++;
            }

            return positions;
        }

        private List<Box> AddBottomLeft(List<Box> positions)
        {
            int row = box.Row - 1;
            int col = box.Column - 1;

            while (row >= 0 && col >= 0)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(row, col).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(row, col));
                    if (piece != null)
                        break;
                }
                else
                    break;
                row--;
                col--;
            }
           
            return positions;
        }

        private List<Box> AddBottomRight(List<Box> positions)
        {
            int row = box.Row - 1;
            int col = box.Column + 1;

            while (row >= 0 && col <8)
            {
                Piece piece = BoardController.instance.ChessBoard.GetBoxInternal(row, col).GetPiece();
                if (piece == null || piece.Color != color)
                {
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(row, col));
                    if (piece != null)
                        break;
                }
                else
                    break;
                row--;
                col++;
            }

            return positions;
        }
    }
}
