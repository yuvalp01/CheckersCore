using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace CheckersCore
{
    class Program
    {
        static Player _currentPlayer;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to simple checkers");
            Console.WriteLine("Initializing board");
            Initilizer initilizer = new Initilizer();


            while (true)
            {
                try
                {
                    //White always moves first
                    _currentPlayer = Player.White;
                    Dictionary<Square, Player> board = initilizer.InitBoard();
                    Game game = new Game(board);
                    Simulation simulation = new Simulation();
                    List<Movement> movements = new List<Movement>();
                    Console.WriteLine("***Menu***");
                    Console.WriteLine("For BLACK winner simulation enter B");
                    Console.WriteLine("For WHITE winner simulation enter W");
                    Console.WriteLine("For Illegal simulation enter I");
                    Console.WriteLine("For Incomplete simulation enter C");
                    Console.WriteLine("For your own simulation file enter P");
                    Console.WriteLine("*******");
                    string choice = Console.ReadLine();
                    switch (choice.ToUpper())
                    {
                        case "B":
                            movements = simulation.LoadSimulation("..\\..\\Simulations\\black.txt");
                            break;
                        case "W":
                            movements = simulation.LoadSimulation("..\\..\\Simulations\\white.txt");
                            break;
                        case "I":
                            movements = simulation.LoadSimulation("..\\..\\Simulations\\illegal_move.txt");
                            break;
                        case "C":
                            movements = simulation.LoadSimulation("..\\..\\Simulations\\incomplete.txt");
                            break;
                        case "P":
                            Console.WriteLine("Please ented the simulation file path");
                            string path = Console.ReadLine();
                            try
                            {
                                movements = simulation.LoadSimulation(path);
                            }
                            catch (FileNotFoundException)
                            {
                                Console.WriteLine($"The file {path} does not exist");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Illegal file format, please use text file with 4 points in each line separated by comma. Exception: {ex.Message}");
                            }

                            break;
                        case "T":
                            //For testing
                            var emptyBoard = initilizer.InitEmptyBoard();
                            emptyBoard[new Square(5, 4)] = Player.Black;
                            emptyBoard[new Square(4, 3)] = Player.White;
                            emptyBoard[new Square(6, 3)] = Player.White;
                            emptyBoard[new Square(7, 2)] = Player.Black;
                            Game fakeGame = new Game(emptyBoard);

                            var possibilities = fakeGame.GetCapturePossibilities(Player.Black);

                            foreach (var item in possibilities)
                            {
                                Console.WriteLine($"Can capture {item.Value} in {item.Key}");

                            }
                            break;
                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }

                    if (movements.Count > 0)
                    {
                        MoveType moveType = MoveType.Invalid;
                        for (int i = 0; i < movements.Count; i++)
                        {
                            var possibilities = game.GetCapturePossibilities(_currentPlayer);
                            if (possibilities.Count > 0)
                            {
                                foreach (var item in possibilities)
                                {
                                    Console.WriteLine($"Can capture {item.Value} in {item.Key}");

                                }

                            }
                            else
                            {
                                Console.WriteLine($"No capture posibilities for the {_currentPlayer}");
                            }
                            moveType = game.Move(_currentPlayer, movements[i].From, movements[i].To);
                            if (moveType == MoveType.Normal)
                            {
                                _currentPlayer = _currentPlayer == Player.White ? Player.Black : Player.White;
                            }
                            else if (moveType == MoveType.Capture)
                            {
                                //In case player make another capture
                                if ((i + 1) < movements.Count && board[movements[i + 1].From] != _currentPlayer)
                                {
                                    _currentPlayer = _currentPlayer == Player.White ? Player.Black : Player.White;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (moveType != MoveType.Invalid)
                        {
                            PrintStatus(board);
                        }
                        ////For manual testing
                        //PrintBoard(board);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void PrintStatus(Dictionary<Square, Player> board)
        {
            int blacks = board.Count(a => a.Value == Player.Black);
            int whites = board.Count(a => a.Value == Player.White);
            Console.WriteLine("*******");
            Console.WriteLine($"Status: {blacks} blacks left");
            Console.WriteLine($"Status: {whites} whites left");
            if (blacks == 0 || whites == 0)
            {
                if (blacks > whites)
                {
                    Console.WriteLine($"Result: BLACK first, White second");
                }
                else if (blacks < whites)
                {
                    Console.WriteLine($"Result: WHITE first, Blck second");
                }
                else
                {
                    Console.WriteLine($"Result: TIE");
                }
            }
            else
            {
                Console.WriteLine($"INCOMPLETE GAME");

            }
            Console.WriteLine("*******");

        }

        static public void PrintBoard(Dictionary<Square, Player> board)
        {
            foreach (var item in board)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        private static Square GetSquareFromString(string input)
        {
            List<int> points = input.Split(',').Select(a => Convert.ToInt32(a)).ToList();
            return new Square(points[0], points[1]);
        }

    }

}

