using Assets.Scripts;
using Assets.Scripts.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class PieceFactory
{

    public static Piece CreatePiece(Pieces type,ChessColors color, GameObject pieceObject)
    {
        switch (type)
        {
            case Pieces.Pawn:
                return new Pawn(color, pieceObject);
            case Pieces.Rook:
                return new Rook(color, pieceObject);
            case Pieces.Knight:
                return new Knight(color, pieceObject);
            case Pieces.Bishop:
                return new Bishop(color, pieceObject);
            case Pieces.Queen:
                return new Queen(color, pieceObject);
            case Pieces.King:
                return new King(color, pieceObject);
        }
        throw new Exception("Type not supported");

    }

}
