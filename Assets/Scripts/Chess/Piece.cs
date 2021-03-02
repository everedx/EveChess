using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Chess
{
    public abstract class Piece : IPiece
    {
        protected Box box;
        protected Pieces type;
        protected ChessColors color;
        protected GameObject pieceObject;

        public Box Box { get => box;  }
        public Pieces Type { get => type; }
        public ChessColors Color { get => color; }
        public GameObject PieceObject { get => pieceObject; }

        public abstract List<Box> CheckPossibleMovements();
        public abstract void Move(Box destination);

        public override string ToString()
        {
            return "Type: " + type + " ,Color : " + color;
        }

        public void SetBox(Box box)
        {
            this.box = box;
        }
    }
}
