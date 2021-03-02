﻿using Assets.Scripts.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Board
    {

        Box[,] boxes = new Box[8,8];

        public Board()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boxes[i, j] = new Box(i,j);

                }
            }
        }


        public Box GetBoxInternal(int row, int column)
        {
            return boxes[row, column];
        }

        public Box GetBoxChessCoordinates(char row, int column)
        {
            return GetBoxInternal(column - 1, row - 'A');
        }

        public Box GetBoxChessCoordinatesNet(char col, char row)
        {
            return GetBoxInternal(row - 'A', col - 'A');
        }


        public void AddPiece(int row, int column, Piece piece)
        {
            GetBoxInternal(row, column).SetPiece(piece);
        }

    }

 
    
    
    
    public enum Pieces
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public enum ChessColors
    {
        White,
        Black
    }
}