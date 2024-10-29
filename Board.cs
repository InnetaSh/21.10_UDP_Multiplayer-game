using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _21._10_UDP_Multiplayer_game
{
    internal class Board
    {
        public int StartX;
        public int StartY;
        public static int Width = 20;
        public static int Height = 40;
        public Cell[,] CellMas = new Cell[Width, Height];

        public Board()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                    CellMas[i, j] = new Cell(i, j);
            }
        }


        public Cell FindCurPosCell()
        {
            Cell result = null;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                    if(CellMas[i, j].State == CellState.MyOccupied)
                    {
                        result = CellMas[i, j];
                        break;
                    }
                if(result != null) break;
            }
            return result;
        }

        public void Print()
        {
            Console.Clear();
            Console.WriteLine("    12345678910111213141516171819202122232425262728293031323334353637383940");
            for (int i = 0; i < Width; i++)
            {
                Console.Write($"{i + 1,3} ");
                for (int j = 0; j < Height; j++)
                {
                    if (CellMas[i, j].State == CellState.Empty)
                        Console.Write(' ');
                    else if (CellMas[i, j].State == CellState.Occupied)
                        Console.Write('X');
                    else if (CellMas[i, j].State == CellState.MyOccupied)
                        Console.Write('O');
                }
                Console.WriteLine();
            }
        }


        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public void CellMove(Direction direction)
        {
            var curCell = FindCurPosCell();
            var x = curCell.X;
            var y = curCell.Y;
            switch (direction)
            {
                case Direction.Up:
                    if (x == 0)
                        { 
                        Console.WriteLine("Движение вверх не возможно, выйдете за пределы поля");
                        Thread.Sleep(2000);
                    }
                    else
                        x--;
                    break;
                case Direction.Down:
                    if (x == Width)
                    { 
                        Console.WriteLine("Движение вниз не возможно, выйдете за пределы поля");
                        Thread.Sleep(2000);
                    }
                    else
                        x++;
                    break;
                case Direction.Left:
                        if (y == 0 )
                        { 
                        Console.WriteLine("Движение влево не возможно, выйдете за пределы поля");
                        Thread.Sleep(2000);
                    }
                    else
                        y--;
                    break;
                case Direction.Right:
                    if ( y == Height)
                        { 
                        Console.WriteLine("Движение вниз не возможно, выйдете за пределы поля");
                        Thread.Sleep(2000);
                    }
                    else
                        y++;
                    break;
            }
            var nextCell = CellMas[x, y];
            if (nextCell.State != CellState.Occupied)
            {
                curCell.State = CellState.Empty;
                nextCell.State = CellState.MyOccupied;
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        if (CellMas[i, j].State == CellState.Occupied || CellMas[i, j].State == CellState.MyOccupied) continue;
                        CellMas[i, j].State = CellState.Empty;
                    }
                }

            }
            else
            {
                Console.WriteLine("Клетка занята другим игроком");
                Thread.Sleep(1000);
            }

        }
        

    }
}
