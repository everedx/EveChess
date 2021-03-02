using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Chess
{
    public class Box
    {
        int row;
        int column;

        Piece piece;

        public int Row { get => row;  }
        public int Column { get => column; }

        public char ChessColumnCoord { get => ((char)(column+'A')); }
        public char ChessRowCoord { get => ((char)(row + 'A')); }

        public Box(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public override string ToString()
        {
            return "Row: " + row + ", Column: " + column;
        }

        public void SetPiece(Piece piece)
        {

            this.piece = piece;
            if (piece != null)
            {
                this.piece.SetBox(this);
                Debug.LogWarning("Added -->  " + piece.ToString() + "  ---> " + this.ToString());
            }
            else
                Debug.LogWarning("Removed piece ---> " + this.ToString());



        }
        public void RemovePiece()
        {
            this.piece = null;
        }

        public Piece GetPiece()
        {
            return piece;
        }


    }
}
