using System;
using System.Collections.Generic;
using BowlingScoring.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScoringTests
{
    [TestClass]
    public class ScoreKeeperTests
    {
        private ScoreKeeper _scoreKeeper;


        [TestInitialize]
        public void Init()
        {
            _scoreKeeper = new ScoreKeeper();
        }

        [TestMethod]
        public void ScoreFrame_Roll3And6_FrameTotalEquals9()
        {
            var frame = new BowlingFrame() { FirstRollScore = 3, SecondRollScore = 6 };

            _scoreKeeper.ScoreGame(new BowlingFrame[] { frame });

            Assert.AreEqual(frame.TotalFrameScore, 9);
        }

        [TestMethod]
        public void ScoreFrame_SpareRolled_FirstFrameTotalEquals13()
        {
            var frame = new BowlingFrame() { FirstRollScore = 5, SecondRollScore = 5 };
            var secondFrame = new BowlingFrame() { FirstRollScore = 3, SecondRollScore = 2 };

            _scoreKeeper.ScoreGame(new BowlingFrame[] { frame, secondFrame });

            Assert.AreEqual(13, frame.TotalFrameScore);
        }

        [TestMethod]
        public void ScoreFrame_StrikeRolled_FirstFrameTotalEquals17()
        {
            var frame = new BowlingFrame() { FirstRollScore = 10};
            var secondFrame = new BowlingFrame() { FirstRollScore = 4, SecondRollScore = 3 };

            _scoreKeeper.ScoreGame(new BowlingFrame[] { frame, secondFrame });
            Assert.AreEqual(17, frame.TotalFrameScore);
        }

        [TestMethod]
        public void ScoreGame_AllStrikes_TotalScoreEquals300()
        {
            var frames = new BowlingFrame[10];
            for (int i = 0; i < 10; i++)
            {
                frames[i] = new BowlingFrame() { FirstRollScore = 10 };
            }
            frames[9].IsTenthFrame = true;
            frames[9].SecondRollScore = 10;
            frames[9].BonusRollScore = 10;

            _scoreKeeper.ScoreGame(frames);
            //300 is the maximum number of points in bowling
            Assert.AreEqual(300, _scoreKeeper.TotalScore);

        }

        private BowlingFrame _strikeFrameNeedingScore;

        private bool IsStrike(BowlingFrame frame)
        {
            return frame.FirstRollScore == 10;
        }

        private bool IsSpare(BowlingFrame frame)
        {
            return frame.FirstRollScore + frame.SecondRollScore == 10;
        }
        [TestMethod]
        public void ScoreGame_RollTwoStrikes_FirstFrameScore25()
        {
            var game = new BowlingFrame[3];

            game[0] = new BowlingFrame() { FirstRollScore = 10, FinishedEnteringRolls = true };
            game[1] = new BowlingFrame() { FirstRollScore = 10, FinishedEnteringRolls = true };
            game[2] = new BowlingFrame() { FirstRollScore = 5, FinishedEnteringRolls = true };

            _scoreKeeper.ScoreGame(game);

            Assert.AreEqual(25, game[0].TotalFrameScore);
        }
    }
}
