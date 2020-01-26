using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoring.Model
{
    //This class keeps track of all frames in the game and returns the total score
    public class ScoreKeeper
    {
        public ScoreKeeper()
        {

        }

        public int TotalScore;
        
        private const int NUMBER_OF_PINS = 10;
      
        public void ScoreGame(BowlingFrame[] game)
        {

            var currentFrameScore = 0;
            var runningScore = 0;
            for (int i = 0; i < game.Length; i++)
            {
                var frame = game[i];

                if (!frame.FinishedEnteringRolls) break;
                if (frame.ScoringFinished)
                {
                    runningScore = frame.TotalFrameScore;
                    continue;

                }
                if (frame.IsTenthFrame)
                {
                    //the scoring of the last frame is different and a little tricky, so we score it seperately from the rest
                    ScoreTenthFrame(frame);
                    continue;
                }

                 if (IsStrike(frame))
                {
                    currentFrameScore = frame.FirstRollScore;
                    //if the current frame is a strike AND the next frame is a strike, we also need the first roll from two frames ahead
                    var nextFrame = game[i + 1];
                    if (nextFrame.FinishedEnteringRolls) //We want to update the view in real time, but only after the scoring of the two bonus rolls are done.
                    {
                        if (IsStrike(nextFrame) && nextFrame.IsTenthFrame) //the scoring of the tenth frame is tricky and also affects the ninth
                        {
                            currentFrameScore = frame.FirstRollScore + nextFrame.FirstRollScore + nextFrame.SecondRollScore;

                            frame.ScoringFinished = true;
                        }
                        else if (IsStrike(nextFrame) && game[i + 2].FinishedEnteringRolls)
                        {
                            currentFrameScore = frame.FirstRollScore + nextFrame.FirstRollScore + game[i + 2].FirstRollScore;
                            frame.ScoringFinished = true;
                        }
                        else
                        {
                            currentFrameScore = frame.FirstRollScore + nextFrame.FirstRollScore + nextFrame.SecondRollScore;
                        }                        
                    }
                    else
                    {
                        currentFrameScore += nextFrame.FirstRollScore; //The next frame may not be finished, but it could have it's first roll entered, in which case we are still updating the score
                    }
                    
                }
                else if (IsSpare(frame))
                {
                    //TODO: update score after a spare is entered (in event that game is finished before 10th frame)
                    currentFrameScore = NUMBER_OF_PINS + game[i + 1].FirstRollScore;
                    frame.ScoringFinished = true;
                }
                else
                {
                    currentFrameScore = frame.FirstRollScore + frame.SecondRollScore;
                    frame.ScoringFinished = true;
                }
                
                frame.TotalFrameScore = currentFrameScore + runningScore;
                runningScore += currentFrameScore;
            }
        }


        private void ScoreTenthFrame(BowlingFrame frame)
        {            
            //todo: finish tenth frame logic
            if (IsStrike(frame) || IsSpare(frame))
            {
                frame.FinishedEnteringRolls = false;
                frame.HasTenthFrameBonus = true;
                frame.TotalFrameScore = frame.FirstRollScore + frame.SecondRollScore + frame.BonusRollScore;
                TotalScore += frame.TotalFrameScore;
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

        // Because multiple strikes can happen in a row, we need to filter out scores where the first roll is a strike and the second is a 0
        private int CalculateBonusPoints(IEnumerable<BowlingFrame> frames)
        {
            var score = 0;
            var countOfBonuses = 0;

            foreach (var frame in frames)
            {
                if (countOfBonuses == 2) return score;

                if (frame.IsTenthFrame)
                {
                    return frame.FirstRollScore + frame.SecondRollScore;
                }
                else
                {
                    if (IsStrike(frame))
                    {
                        score += frame.FirstRollScore;
                        countOfBonuses++;
                    }
                    else
                    {
                        return frame.FirstRollScore + frame.SecondRollScore;
                    }
                }
            }

            //foreach (var frame in frames)
            //{
            //    if (countOfBonuses != 2)
            //    {
            //        if ()

            //        //if (frame.RollOneScore == NUMBER_OF_PINS)
            //        //{
            //        //    score += frame.RollOneScore;
            //        //    countOfBonuses++;
            //        //}
            //        //else
            //        //{
            //        //    if (frame.IsTenthFrame)
            //        //    return frame.RollOneScore + frame.RollTwoScore;
            //        //}
                    
            //    }
            //}
            return score;
        }
    }
}
