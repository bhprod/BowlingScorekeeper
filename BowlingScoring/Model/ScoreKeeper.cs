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
            //_frames = new Frame[10];
            _framesAwaitingScoring = new BowlingFrame[2];
            _currentFrameIndex = 0;
        }

        //functions: scores ball rolls
        // a strike is 10 points
        // a spare is however many points cleared the track
        // A game of bowling consists of 10 frames with 12 maximum rolls, with a maximum score of 300 points

        //private Frame[] _frames;

        public int TotalScore;
        
        private const int NUMBER_OF_PINS = 10;
       
        private int[] GetBonusRolls(BowlingFrame[] game)
        {
            var bonus = new int[2];
            bonus[0] = game[0].FirstRollScore;

            if (IsStrike(game[0]) && game[0].IsTenthFrame)
            {
                bonus[1] = game[0].SecondRollScore;
                return bonus;
            }            
            if (IsStrike(game[0]))
            {
                bonus[1] = game[1].FirstRollScore;
            }
            return bonus;
        }

        public void ScoreGame(BowlingFrame[] game)
        {

            var currentFrameScore = 0;
            for (int i = 0; i < game.Length; i++)
            {
                var frame = game[i];

                if (frame.ScoringFinished) continue;
                if (frame.IsTenthFrame)
                {
                    //the scoring of the last frame is different and a little tricky, so we score it seperately from the rest
                    ScoreTenthFrame(frame);
                    continue;
                }
                if (!frame.FinishedEnteringRolls) continue;

                
                if (IsStrike(frame))
                {                    
                    //if the current frame is a strike AND the next frame is a strike, we also need the first roll from two frames ahead
                    var nextFrame = game[i + 1];
                    if (nextFrame.FinishedEnteringRolls) //We want to update the view in real time, but only after the scoring of the two bonus rolls are done.
                    {
                        if (IsStrike(nextFrame) && nextFrame.IsTenthFrame) //the scoring of the tenth frame is tricky and also affects the ninth
                        {
                            currentFrameScore = frame.FirstRollScore + nextFrame.FirstRollScore + nextFrame.SecondRollScore;
                        }
                        else if (IsStrike(nextFrame) && game[i + 2].FinishedEnteringRolls)
                        {
                            currentFrameScore = frame.FirstRollScore + nextFrame.FirstRollScore + game[i + 2].FirstRollScore;
                        }
                        else
                        {
                            currentFrameScore = frame.FirstRollScore + nextFrame.FirstRollScore + nextFrame.SecondRollScore;
                        }

                        frame.ScoringFinished = true;
                    }

                }
                else if (IsSpare(frame))
                {
                    if (game[i + 1].FinishedEnteringRolls)
                    {
                        currentFrameScore = 10 + game[i + 1].FirstRollScore;
                        frame.ScoringFinished = true;
                    }
                }
                else
                {
                    currentFrameScore = frame.FirstRollScore + frame.SecondRollScore;
                    frame.ScoringFinished = true;
                }
                frame.TotalFrameScore = currentFrameScore + TotalScore;
                TotalScore += currentFrameScore;
            }
        }


        private void ScoreTenthFrame(BowlingFrame frame)
        {            
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

        //If a strike is rolled we need to able to reference a maximum of two more frames. If two consecutive strikes are rolled we need the next frame as well.
        private void SubscribedFrame_ScoreFinished(object sender, EventArgs e)
        {
            
        }

        private BowlingFrame[] _framesAwaitingScoring;
        private int _currentFrameIndex;
        private void ScoreFrame(BowlingFrame frame) { 
            
            if (IsStrike(frame) || IsSpare(frame))
            {
                _framesAwaitingScoring[_currentFrameIndex] = frame;
                _currentFrameIndex++;
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
