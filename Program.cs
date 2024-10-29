using System.Runtime.CompilerServices;
using static _21._10_UDP_Multiplayer_game.Board;

namespace _21._10_UDP_Multiplayer_game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
           Run();

            Console.ReadKey();
        }

        private static async Task Run()
        {
            var board = new Board();
            Client client = new Client("235.5.5.11", 12345, board);
           
           
            StartMenu(board);

            Task.Run(client.ReceiveMessageAsync);
            board.Print();
            board.CellMas[board.StartX, board.StartY].State = CellState.MyOccupied;

            

            client.SendMessage($"{board.StartX+1}", $"{board.StartY+1}");
           

            board.Print();
            var flag = false;
            while (!flag)
            {
                switch (GetCommand(board))
                {
                    case CommandState.Move:
                        var curCell = board.FindCurPosCell();
                        client.SendMessage($"{curCell.X+1}", $"{curCell.Y+1}");




                        board.Print(); break;
                    case CommandState.Escape: flag = true; break;
                }
            }
        }

        static CommandState GetCommand(Board board)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                key.KeyChar.ToString().ToUpper();
                switch (key.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        board.CellMove(Direction.Up);
                        return CommandState.Move;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        board.CellMove(Direction.Left);
                        return CommandState.Move;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        board.CellMove(Direction.Down);
                        return CommandState.Move;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        board.CellMove(Direction.Right);
                        return CommandState.Move;
                    case ConsoleKey.Escape:
                        return CommandState.Escape;
                }
            }
            return CommandState.None;
        }

        private static void StartMenu(Board board)
        {
           
                Console.WriteLine("    12345678910111213141516171819202122232425262728293031323334353637383940");
                for (int i = 0; i < Width; i++)
                {
                    Console.Write($"{i + 1,3} ");
                    for (int j = 0; j < Height; j++)
                    {
                        if (board.CellMas[i, j].State == CellState.Empty)
                            Console.Write(' ');
                        else if (board.CellMas[i, j].State == CellState.Occupied)
                            Console.Write('X');
                        else if (board.CellMas[i, j].State == CellState.MyOccupied)
                            Console.Write('O');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("-------------------------------------------------------------------------------");

           
            
                Console.WriteLine("Выберите стартовую координату X");

                while (!Int32.TryParse(Console.ReadLine(), out board.StartX) || board.StartX < 1 || board.StartX > 40)
                {
                    Console.WriteLine("Не верный ввод.Введите координату X:");
                }
                board.StartX = board.StartX - 1;

                Console.WriteLine("Выберите стартовую координату Y");

                while (!Int32.TryParse(Console.ReadLine(), out board.StartY) || board.StartY < 1 || board.StartY > 20)
                {
                    Console.WriteLine("Не верный ввод.Введите координату Y:");
                }
                board.StartY = board.StartY - 1;

            Console.CursorVisible = false;



        }
    }

    enum CommandState
    {
        None,
        Move,
        Escape
    }
}
