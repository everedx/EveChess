using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Chess
{
    class Knight:Piece
    {
        public Knight(ChessColors color, GameObject pieceObject)
        {
            this.pieceObject = pieceObject;
            type = Pieces.Knight;
            this.color = color;
        }

        public override List<Box> CheckPossibleMovements()
        {
            return GetKnightPositions();
        }

        public override void Move(Box destination)
        {
            throw new NotImplementedException();
        }


        private List<Box> GetKnightPositions()
        {
            List<Box> positions = new List<Box>();

            if (box.Row + 1 < 8 && box.Column + 2 < 8) 
            {
                if (IsPositionAvailable(BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 1, box.Column + 2)))
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 1, box.Column + 2)); 
            }

            if (box.Row + 2 < 8 && box.Column + 1 < 8)
            {
                if (IsPositionAvailable(BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 2, box.Column + 1)))
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 2, box.Column + 1));
            }

            if (box.Row - 1 >=0  && box.Column + 2 < 8)
            {
                if (IsPositionAvailable(BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 1, box.Column + 2)))
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 1, box.Column + 2));
            }

            if (box.Row - 2 >= 0 && box.Column + 1 < 8)
            {
                if (IsPositionAvailable(BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 2, box.Column + 1)))
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 2, box.Column + 1));
            }

            if (box.Row + 1 < 8 && box.Column - 2 >=0)
            {
                if (IsPositionAvailable(BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 1, box.Column - 2)))
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 1, box.Column - 2));
            }

            if (box.Row + 2 < 8 && box.Column - 1 >= 0)
            {
                if (IsPositionAvailable(BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 2, box.Column - 1)))
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 2, box.Column - 1));
            }

            if (box.Row - 1 >= 0 && box.Column - 2 >=0)
            {
                if (IsPositionAvailable(BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 1, box.Column - 2)))
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 1, box.Column - 2));
            }

            if (box.Row - 2 >= 0 && box.Column - 1 >= 0)
            {
                if (IsPositionAvailable(BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 2, box.Column - 1)))
                    positions.Add(BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 2, box.Column - 1));
            }

            return positions;
        }

        private bool IsPositionAvailable(Box box)
        {
            if (box.GetPiece() == null || box.GetPiece().Color != color)
                return true;
            else
                return false;
        }
    }
}
