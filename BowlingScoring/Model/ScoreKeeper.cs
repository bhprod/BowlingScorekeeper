using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoring.Model
{
    /// <summary>
    /// Tracks the overall score of the game by adding the scores of each frame.
    /// </summary>
    public class ScoreKeeper
    {
        public ScoreKeeper() { }

        private const int STRIKE_VALUE = 10;

        public void ScoreGame(BowlingFrame[] game)
        {
            var currentFrameScore = 0;
            var gameScore = 0;
            for (int i = 0; i < game.Length; i++)
            {
                var frame = game[i];
                if (!frame.FinishedEnteringRolls && !frame.IsTenthFrame) break;
                if (frame.FinishedScoring)
                {
                    gameScore = frame.TotalFrameScore;
                    continue;
                }
                if (frame.IsTenthFrame) //the scoring of the last frame is different and a little tricky, so we score it seperately from the rest
                {
                    frame.TotalFrameScore = ScoreTenthFrame(frame) + gameScore;
                    break;
                }

                if (IsStrike(frame)) { currentFrameScore = ScoreStrike(game, i); }

                else if (IsSpare(frame)) { currentFrameScore = ScoreSpare(game, i); }

                else
                {
                    currentFrameScore = frame.FirstRollScore + frame.SecondRollScore;
                    frame.FinishedScoring = true;
                }

                frame.TotalFrameScore = currentFrameScore + gameScore;
                gameScore += currentFrameScore;
            }
        }

        private int ScoreSpare(BowlingFrame[] game, int currentIndex)
        {
            if (game[currentIndex + 1].FirstRollScore > 0)
            {
                game[currentIndex].FinishedScoring = true;
                return STRIKE_VALUE + game[currentIndex + 1].FirstRollScore;

            }
            else { return STRIKE_VALUE; }
        }

        private int ScoreStrike(BowlingFrame[] game, int i)
        {
            var currentFrame = game[i];
            currentFrame.FinishedEnteringRolls = true;
            var currentFrameScore = currentFrame.FirstRollScore;
            var nextFrame = game[i + 1];
            var nextFrameFirstRollValue = nextFrame.FirstRollScore;

            if (nextFrame.FinishedScoring || nextFrame.IsTenthFrame) //We want to update the score in real time, if the next frame is a strike then it will not be finished entering roll
            {
                if (IsStrike(nextFrame) && nextFrame.IsTenthFrame) //the scoring of the tenth frame is tricky and also affects the ninth
                {
                    currentFrameScore = currentFrame.FirstRollScore + nextFrameFirstRollValue + nextFrame.SecondRollScore;
                }
                else if (IsStrike(nextFrame) && game[i + 2].FinishedEnteringRolls)
                {
                    currentFrameScore = currentFrame.FirstRollScore + nextFrameFirstRollValue + game[i + 2].FirstRollScore;
                    currentFrame.FinishedScoring = true;
                }
                else
                {
                    currentFrameScore = currentFrame.FirstRollScore + nextFrameFirstRollValue + nextFrame.SecondRollScore;
                }
            }
            else
            {
                var secondBonusRoll = IsStrike(nextFrame) ? game[i + 2].FirstRollScore : nextFrame.SecondRollScore;
                currentFrameScore += nextFrameFirstRollValue + secondBonusRoll; //The next frame may not be finished, but it could have it's first roll entered, in which case we are still updating the score
            }
            return currentFrameScore;
        }

        private int ScoreTenthFrame(BowlingFrame frame)
        {
            if (IsStrike(frame) || IsSpare(frame))
            {
                frame.FinishedEnteringRolls = false;
                frame.HasTenthFrameBonus = true;
            }
            else
            {
                frame.HasTenthFrameBonus = false;
            }

            if (frame.SecondRollScore == STRIKE_VALUE)
            {
                frame.FinishedEnteringRolls = false;
            }

            if (frame.HasTenthFrameBonus && !string.IsNullOrEmpty(frame.BonusRollString))
            {
                frame.FinishedEnteringRolls = true;
                frame.FinishedScoring = true;
            }

            return frame.FirstRollScore + frame.SecondRollScore + frame.BonusRollScore;
        }

        private bool IsStrike(BowlingFrame frame)
        {
            return frame.FirstRollScore == STRIKE_VALUE;
        }

        private bool IsSpare(BowlingFrame frame)
        {
            return frame.FirstRollScore + frame.SecondRollScore == STRIKE_VALUE;
        }


    }
}
