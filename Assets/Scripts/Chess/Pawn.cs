using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Chess
{
    class Pawn : Piece
    {
        private bool isFirstMovement = true;
        private bool enPassant;
        public bool EnPassant { get => enPassant; }


        public Pawn(ChessColors color, GameObject pieceObject)
        {
            type = Pieces.Pawn;
            this.pieceObject = pieceObject;
            this.color = color;
            enPassant = false;
            isFirstMovement = true;
        }


        public override List<Box> CheckPossibleMovements()
        {
            List<Box> positions = new List<Box>();

            switch (color)
            {
                case ChessColors.White:
                    {
                        Box frontBox = BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 1, box.Column);

                        Box leftBox = null, righBox = null, frontLeft = null, frontRight = null;

                        if (frontBox.GetPiece() == null)
                            positions.Add(frontBox);

                        if (box.Row + 1 < 8 && box.Column + 1 < 8) frontRight = BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 1, box.Column + 1);
                        if (box.Row + 1 < 8 && box.Column - 1 >= 0) frontLeft = BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 1, box.Column - 1);
                        if (box.Column - 1 >= 0) leftBox = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column - 1);
                        if (box.Column + 1 < 8) righBox = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column + 1);

                        if (frontRight != null && frontRight.GetPiece() != null && frontRight.GetPiece().Color != color)
                            positions.Add(frontRight);
                        if (frontLeft != null && frontLeft.GetPiece() != null && frontLeft.GetPiece().Color != color)
                            positions.Add(frontLeft);

                        if (isFirstMovement)
                        {
                            Box frontFrontBox = BoardController.instance.ChessBoard.GetBoxInternal(box.Row + 2, box.Column);
                            if (frontFrontBox.GetPiece() == null && positions.Contains(frontBox))
                                positions.Add(frontFrontBox);
                        }
                    }
                    break;
                case ChessColors.Black:
                    {
                        Box frontBox = BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 1, box.Column);
                        Box leftBox = null, righBox = null, frontLeft = null, frontRight = null;
                        if (frontBox.GetPiece() == null)
                            positions.Add(frontBox);


                        if (box.Row - 1 >= 0 && box.Column - 1 >= 0) frontRight = BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 1, box.Column - 1);
                        if (box.Row - 1 >= 0 && box.Column + 1 <8) frontLeft = BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 1, box.Column + 1);
                        if (box.Column + 1 < 8) leftBox = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column + 1);
                        if (box.Column - 1 >= 0) righBox = BoardController.instance.ChessBoard.GetBoxInternal(box.Row, box.Column - 1);

                        if (frontRight != null && frontRight.GetPiece() != null && frontRight.GetPiece().Color != color)
                            positions.Add(frontRight);
                        if (frontLeft != null && frontLeft.GetPiece() != null && frontLeft.GetPiece().Color != color)
                            positions.Add(frontLeft);

                        if (isFirstMovement)
                        {
                            Box frontFrontBox = BoardController.instance.ChessBoard.GetBoxInternal(box.Row - 2, box.Column);
                            if (frontFrontBox.GetPiece() == null && positions.Contains(frontBox))
                                positions.Add(frontFrontBox);
                        }

                    }
                    break;
            }
           

            return positions;

        }

        public void AckFirstMovement()
        {
            isFirstMovement = false;
        }
            

        public override void Move(Box destination)
        {
            throw new NotImplementedException();
        }
    }
}
