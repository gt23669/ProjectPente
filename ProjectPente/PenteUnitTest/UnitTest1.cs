using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectPente;
using ProjectPente.Models;

namespace PenteUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MainWindow window = new MainWindow();
            GameController gc = new GameController("1stPlayer", "2ndPlayer", Mode.PVP, window);
            var expected = gc.player2;
            var actual = gc.CurrentPlayer;
            gc.ChangePlayer();
            //Assert.AreNotSame(gc.player1,currentPlayer);
            Assert.AreNotSame(expected, actual);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Tuple<int, int> center = new Tuple<int, int>(10,10);
            Tuple<int, int> testing = new Tuple<int, int>(10, 10);
            MainWindow window = new MainWindow();
            GameController gc = new GameController("1stPlayer", "2ndPlayer", Mode.PVP, window);
            var expected = false;
            var actual = gc.OutsideCenter(testing,center);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod3()
        {
            MainWindow window = new MainWindow();
            GameController gc = new GameController("1stPlayer", "2ndPlayer", Mode.PVP, window);
            var expected = gc.GetTiles();
            var actual = typeof(List<Tile>);
            Assert.IsInstanceOfType(expected, actual);
            //Assert.IsInstanceOfType(obj, typeof(MyObject));
        }
    }
}
