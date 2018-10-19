using System;
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
            GameController gc = new GameController("1stPlayer","2ndPlayer",Mode.PVP,new Tuple<int,int>(10,10), window);
            var expected = gc.player2;
            var actual = gc.CurrentPlayer;
            gc.TogglePlayer();
            //Assert.AreNotSame(gc.player1,currentPlayer);
            Assert.AreNotSame(expected,actual);
            
            
        }
    }
}
