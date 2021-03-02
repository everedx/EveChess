using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Chess
{
    class King : Piece
    {
        public King(ChessColors color, GameObject pieceObject)
        {
            this.pieceObject = pieceObject;
            type = Pieces.King;
            this.color = color;
        }

        public override List<Box> CheckPossibleMovements()
        {
            throw new NotImplementedException();
        }

        public override void Move(Box destination)
        {
            throw new NotImplementedException();
        }
    }
}
