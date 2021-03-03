using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Chess
{
    class Rook : Piece
    {
        private bool isFirstMovement = true;
        public bool IsFirstMovement { get => isFirstMovement; }
        public Rook(ChessColors color, GameObject pieceObject)
        {
            type = Pieces.Rook;
            this.pieceObject = pieceObject;
            this.color = color;
            isFirstMovement = true;
        }

        public override List<Box> CheckPossibleMovements()
        {
            List<Box> positions = new List<Box>();
            positions = AddVerticalPositions(positions);
            positions = AddHorizontalPositions(positions);
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

        public void AckFirstMovement()
        {
            isFirstMovement = false;

        }
    }
}
