using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Chess
{
    class Bishop : Piece
    {
        public Bishop(ChessColors color, GameObject pieceObject)
        {
            this.pieceObject = pieceObject;
            type = Pieces.Bishop;
            this.color = color;
        }

        public override List<Box> CheckPossibleMovements()
        {
            List<Box> positions = new List<Box>();

            positions = AddObliquePositions(positions);

            return positions;
        }

        public override void Move(Box destination)
        {
            throw new NotImplementedException();
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

            while (row >= 0 && col < 8)
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
