using System;
using BowlingScoring.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScoringTests
{
    [TestClass]
    public class BowlingFrameTests
    {
        int STRIKE_VALUE = 10;

        [TestMethod]
        public void FirstRollString_Strike_FirstRollScore10()
        {
            var frame = new BowlingFrame() { FirstRollString = "x" };
            Assert.AreEqual(STRIKE_VALUE, frame.FirstRollScore);
        }

        [TestMethod]
        public void FirstRollString_Strike_SecondRollDash()
        {
            var frame = new BowlingFrame() { FirstRollString = "x" };
            Assert.IsTrue(frame.SecondRollString.Equals("-"));
        }

        [TestMethod]
        public void FirstRollString_Scratch_FirstRollScore0()
        {
            var frame = new BowlingFrame() { FirstRollString = "-" };
            Assert.AreEqual(0, frame.FirstRollScore);
        }

        [TestMethod]
        public void FirstRollString_12_FirstRollScore0() 
        {
            var frame = new BowlingFrame() { FirstRollString = "12" };
            Assert.AreEqual(0, frame.FirstRollScore);
        }

        [TestMethod]
        public void FirstRollString_Spare_FirstRollScore0()
        {
            var frame = new BowlingFrame() { FirstRollString = "/" };
            Assert.AreEqual(0, frame.FirstRollScore);
        }

        [TestMethod]
        public void SecondRollString_Spare_FirstSecondTotal10()
        {
            var frame = new BowlingFrame() { FirstRollString = "1", SecondRollString = "/" };
            Assert.AreEqual(STRIKE_VALUE, frame.FirstRollScore + frame.SecondRollScore);
        }

        [TestMethod]
        public void FirstRoll2_SecondRollScratch_FirstSecondTotal2()
        {
            var frame = new BowlingFrame() { FirstRollString = "2", SecondRollString = "-" };
            Assert.AreEqual(2, frame.FirstRollScore + frame.SecondRollScore);
        }
    }
}
