using System;
using System.Collections.Generic;
using CheckersCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace UnitTestChecker
{
    [TestClass]
    public class BasicUnitTest
    {

        [TestMethod]
        public void CaptureMovementCompleted()
        {
            //Arrange
            Initilizer initilizer = new Initilizer();
            Dictionary<Square, Player> board = initilizer.InitEmptyBoard();
            board[new Square(1, 2)] = Player.White;
            board[new Square(2, 3)] = Player.Black;

            int countBlacksBefore = board.Count(a => a.Value == Player.Black);
            Assert.IsTrue(countBlacksBefore == 1);

            Square from = new Square(1, 2);
            Square to = new Square(3, 4);

            Game game = new Game(board);
            //Act:
            game.Move(Player.White, from, to);
            Assert.IsTrue(board[from] == Player.None);
            Assert.IsTrue(board[to] == Player.White);

            Square between = new Square((from.X + to.X) / 2, (from.Y + to.Y) / 2);

            Assert.IsTrue(board[between] == Player.None);
            int countBlacks = board.Count(a => a.Value == Player.Black);
            Assert.IsTrue(countBlacks == 0);

        }

        [TestMethod]
        public void NormalMovementCompleted()
        {
            //Arrange
            Initilizer initilizer = new Initilizer();
            Dictionary<Square, Player> board = initilizer.InitEmptyBoard();
            board[new Square(1, 2)] = Player.White;
            Square from = new Square(1, 2);
            Square to = new Square(2, 3);

            Game game = new Game(board);
            //Act:
            game.Move(Player.White, from, to);
            Assert.IsTrue(board[from]== Player.None);
            Assert.IsTrue(board[to]== Player.White);

        }

        [TestMethod]
        public void MovementToOccupiedSquare()
        {
            //Arrange
            Initilizer initilizer = new Initilizer();
            Dictionary<Square, Player> board = initilizer.InitEmptyBoard();
            board[new Square(3, 4)] = Player.Black;
            Square from = new Square(4, 3);
            Square to = new Square(3, 4);
            Game validator = new Game(board);
            //Act:
            bool result =   validator.ValidateFeasibility(from, to);
            Assert.IsFalse(result);

        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Square does not exist on the board")]
        public void SquareOutOfRange()
        {
            //Arrange
            Square square = new Square(9, 12);

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "It is not posible to use white squares")]
        public void OnWhiteSquare()
        {
            //Arrange
            Square square = new Square(0, 0);

        }

    }
}
