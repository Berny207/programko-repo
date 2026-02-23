using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Model
{
    internal class GameBoard
    {
        Square[,] board = new Square[8,8];

        public GameBoard()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    board[i, j] = new Square(new Position(i, j));
                }
            }
        }
        public Square GetSquare(Position pos)
        {
            return board[pos.x, pos.y];
        }
    }
}
