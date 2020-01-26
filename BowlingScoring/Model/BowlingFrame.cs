using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoring.Model
{
    public class BowlingFrame : INotifyPropertyChanged
    {
        public BowlingFrame()
        {
        }

        public BowlingFrame(int id)
        {
            _frameId = id;
        }

        private int _frameId;


        private const int NUMBER_OF_PINS = 10;
        private int _rollOneScore;

        private string _firstRollString;

        public string FirstRollString
        {
            get { return _firstRollString; }
            set 
            {
                //TODO update score correctly when backspaced and input reentered, like the second ball
                _firstRollString = value;
                if (value.Equals("x", StringComparison.OrdinalIgnoreCase))
                {
                    FinishedEnteringRolls = true;
                    FirstRollScore = NUMBER_OF_PINS;
                    SecondRollScoreString = (IsTenthFrame) ? "" : "-";
                    _firstRollString = value;
                    
                }
                else if (value.Equals("-"))
                {
                    FirstRollScore = 0;
                    _firstRollString = value;
                }
                else if (int.TryParse(value, out int score))
                {
                    if (score <= 9 && score >= 1)
                    {
                        FirstRollScore = score;
                        _firstRollString = value;
                    }
                }
                OnRollScoreEntered();
                OnPropertyChanged(nameof(FirstRollString));
            }
        }

        private string _secondRollScoreString;

        public string SecondRollScoreString
        {
            get { return _secondRollScoreString; }
            set
            {
                //Sterilize inputs
                if (string.IsNullOrEmpty(value))
                {
                    _secondRollScoreString = "";
                    ScoringFinished = false;
                    return;
                }                

                if (value.Equals("/"))
                {
                    SecondRollScore = NUMBER_OF_PINS - _rollOneScore;
                    _secondRollScoreString = value;
                    FinishedEnteringRolls = true;
                }
                else if (value.Equals("-")) { SecondRollScore = 0; _secondRollScoreString = value; FinishedEnteringRolls = true; }
                else if (int.TryParse(value, out int score))
                {
                    //Validate the entered int and ensure the score can't sum to > 10 
                    if ((_rollOneScore + score > 10) || (score > 9 || score < 1))
                    {
                        _secondRollScoreString = "";
                        return;
                    }
                    else
                    {
                        _secondRollScoreString = value;
                        FinishedEnteringRolls = true;
                        SecondRollScore = score;                        
                    }
                }
                OnPropertyChanged(nameof(SecondRollScoreString));
            }
        }

        public event EventHandler RollScoreEntered;
        public int FirstRollScore
        {
            get { return _rollOneScore; }
            set { 
                _rollOneScore = value;
                OnPropertyChanged(nameof(FirstRollScore));
                if (value == NUMBER_OF_PINS)
                {
                    FinishedEnteringRolls = true;
                }
                OnRollScoreEntered();
            }
        }

        private int _rollTwoScore;

        public int SecondRollScore
        {
            get { return _rollTwoScore; }
            set { _rollTwoScore = value; OnPropertyChanged(nameof(SecondRollScore)); OnRollScoreEntered(); }
        }
        private int _bonusRollScore;

        public int BonusRollScore
        {
            get { return _bonusRollScore; }
            set { _bonusRollScore = value; OnPropertyChanged(nameof(BonusRollScore)); OnRollScoreEntered(); }
        }

        private bool _isTenthFrame;

        public bool IsTenthFrame
        {
            get { return _isTenthFrame; }
            set { _isTenthFrame = value; OnPropertyChanged(nameof(IsTenthFrame)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _finishedEnteringRolls;

        public bool FinishedEnteringRolls
        {
            get { return _finishedEnteringRolls; }
            set { _finishedEnteringRolls = value; OnPropertyChanged(nameof(FinishedEnteringRolls)); }
        }
        //todo: fix when number is entered and then changed the score doesn't update
        private void OnRollScoreEntered()
        {
            RollScoreEntered?.Invoke(this, new EventArgs());
        }

        private int _frameTotalScore;
        public int TotalFrameScore
        {
            get => _frameTotalScore;
            set
            {
                _frameTotalScore = value;
                OnPropertyChanged(nameof(TotalFrameScore));
            }
        }

        public bool ScoringFinished;
        //public void SubscribedFrame_ScoreFinished(object sender, EventArgs e)
        //{
        //    var subscribedFrame = sender as BowlingFrame;

        //    _frameTotalScore += subscribedFrame.RollOneScore;
        //    if (subscribedFrame.RollTwoScore != 0)
        //    {
        //        _frameTotalScore += RollTwoScore;
        //        ScoringFinished = true;
        //        subscribedFrame.RollScoreEntered -= SubscribedFrame_ScoreFinished;
        //    }
        //}
        private bool _hasTenthFrameBonus;

        public bool HasTenthFrameBonus
        {
            get { return _hasTenthFrameBonus; }
            set { _hasTenthFrameBonus = value; OnPropertyChanged(nameof(HasTenthFrameBonus)); }
        }


        //public bool HasTenthFrameBonus => IsTenthFrame && (RollOneScore == NUMBER_OF_PINS || (RollOneScore + RollTwoScore == NUMBER_OF_PINS));

        public int[] BonusRolls;





    }
}
