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
        private const int NUMBER_OF_PINS = 10;
      
        public void ScoreGame(BowlingFrame[] game)
        {
            var currentFrameScore = 0;
            var gameScore = 0;
            for (int i = 0; i < game.Length; i++)
            {
                var frame = game[i];
                if (!frame.FinishedEnteringRolls) break;
                if (frame.ScoringFinished)
                {
                    gameScore = frame.TotalFrameScore;
                    continue;
                }
                if (frame.IsTenthFrame) //the scoring of the last frame is different and a little tricky, so we score it seperately from the rest
                {
                    ScoreTenthFrame(frame);
                    continue;
                }

                if (IsStrike(frame)) { currentFrameScore = ScoreStrike(game, i);}

                else if (IsSpare(frame)) { currentFrameScore = ScoreSpare(game, i); }
                
                else
                {
                    currentFrameScore = frame.FirstRollScore + frame.SecondRollScore;
                    frame.ScoringFinished = true;
                }
                
                frame.TotalFrameScore = currentFrameScore + gameScore;
                gameScore += currentFrameScore;
            }
        }

        private int ScoreSpare(BowlingFrame[] game, int currentIndex)
        {
            if (game[currentIndex + 1].FirstRollScore > 0)
            {
                game[currentIndex].ScoringFinished = true;
                return NUMBER_OF_PINS + game[currentIndex + 1].FirstRollScore;
                
            }
            else { return NUMBER_OF_PINS;}
        }

        private int ScoreStrike(BowlingFrame[] game, int i)
        {
            var frame = game[i];
            var currentFrameScore = frame.FirstRollScore;
            //if the current frame is a strike AND the next frame is a strike, we also need the first roll from two frames ahead
            var nextFrame = game[i + 1];
            var nextFrameFirstRollValue = nextFrame.FirstRollScore;
            if (nextFrame.FinishedEnteringRolls) //We want to update the view in real time, but only after the scoring of the two bonus rolls are done.
            {
                if (IsStrike(nextFrame) && nextFrame.IsTenthFrame) //the scoring of the tenth frame is tricky and also affects the ninth
                {
                    currentFrameScore = frame.FirstRollScore + nextFrameFirstRollValue + nextFrame.SecondRollScore;
                    frame.ScoringFinished = true;
                }
                else if (IsStrike(nextFrame) && game[i + 2].FinishedEnteringRolls)
                {
                    currentFrameScore = frame.FirstRollScore + nextFrameFirstRollValue + game[i + 2].FirstRollScore;
                    frame.ScoringFinished = true;
                }
                else
                {
                    currentFrameScore = frame.FirstRollScore + nextFrameFirstRollValue + nextFrame.SecondRollScore;
                }
            }
            else
            {
                currentFrameScore += nextFrameFirstRollValue; //The next frame may not be finished, but it could have it's first roll entered, in which case we are still updating the score
            }
            return currentFrameScore;
        }


        private void ScoreTenthFrame(BowlingFrame frame)
        {            
            //todo: finish tenth frame logic
            if (IsStrike(frame) || IsSpare(frame))
            {
                frame.FinishedEnteringRolls = false;
                frame.HasTenthFrameBonus = true;
                frame.TotalFrameScore = frame.FirstRollScore + frame.SecondRollScore + frame.BonusRollScore;
                //TotalScore += frame.TotalFrameScore;
                return;
                //frame.TotalFrameScore = frame.RollOneScore + frame.RollTwoScore + frame.BonusRollScore;
            }
            if (frame.SecondRollScore == NUMBER_OF_PINS)
            {
                frame.FinishedEnteringRolls = false;
                //has bonus
            }
        }

        private bool IsStrike(BowlingFrame frame)
        {
            return frame.FirstRollScore == NUMBER_OF_PINS;
        }

        private bool IsSpare(BowlingFrame frame)
        {
            return frame.FirstRollScore + frame.SecondRollScore == NUMBER_OF_PINS;
        }
    }
}
