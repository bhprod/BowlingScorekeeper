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
        //TODO: bug where if scores are entered, then deleted in reverse order the score will not update

        public BowlingFrame() { }

        private const int NUMBER_OF_PINS = 10;

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler RollScoreEntered;
        private void OnRollScoreEntered()
        {
            RollScoreEntered?.Invoke(this, new EventArgs());
        }
        #endregion

        #region First Roll
        private string _firstRollString;
        private int _firstRollScore;
        public int FirstRollScore
        {
            get { return _firstRollScore; }
            set
            {
                _firstRollScore = value;
                OnPropertyChanged(nameof(FirstRollScore));
                if (value == NUMBER_OF_PINS)
                {
                    FinishedEnteringRolls = true;
                }
                OnRollScoreEntered();
            }
        }
        public string FirstRollString
        {
            get { return _firstRollString; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _firstRollString = "";
                    ScoringFinished = false;
                    return;
                }

                if (value.Equals("x", StringComparison.OrdinalIgnoreCase))
                {
                    FinishedEnteringRolls = true;
                    FirstRollScore = NUMBER_OF_PINS;
                    SecondRollString = (IsTenthFrame) ? "" : "-";
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

        #endregion

        #region Second Roll
        private int _rollTwoScore;
        public int SecondRollScore
        {
            get { return _rollTwoScore; }
            set { _rollTwoScore = value; OnPropertyChanged(nameof(SecondRollScore)); OnRollScoreEntered(); }
        }

        private string _secondRollString;
        public string SecondRollString
        {
            get { return _secondRollString; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _secondRollString = "";
                    ScoringFinished = false;
                    return;
                }

                if (value.Equals("/"))
                {
                    _secondRollString = value;
                    FinishedEnteringRolls = true;
                    SecondRollScore = NUMBER_OF_PINS - _firstRollScore;
                }
                else if (value.Equals("-")) { SecondRollScore = 0; _secondRollString = value; FinishedEnteringRolls = true; }
                else if (int.TryParse(value, out int score))
                {
                    if ((_firstRollScore + score > NUMBER_OF_PINS) || (score > 9 || score < 1)) //Validate the int and ensure the score can't sum to > 10 
                    {
                        _secondRollString = "";
                        return;
                    }
                    else
                    {
                        _secondRollString = value;
                        FinishedEnteringRolls = true;
                        SecondRollScore = score;
                    }
                }
                OnPropertyChanged(nameof(SecondRollString));
            }
        }
        #endregion

        #region 10th frame
        private bool _hasTenthFrameBonus;
        public bool HasTenthFrameBonus
        {
            get { return _hasTenthFrameBonus; }
            set { _hasTenthFrameBonus = value; OnPropertyChanged(nameof(HasTenthFrameBonus)); }
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

        #endregion

        private int _totalFrameScore;
        public int TotalFrameScore
        {
            get => _totalFrameScore;
            set
            {
                _totalFrameScore = value;
                OnPropertyChanged(nameof(TotalFrameScore));
            }
        }

        private bool _finishedEnteringRolls;
        public bool FinishedEnteringRolls
        {
            get { return _finishedEnteringRolls; }
            set { _finishedEnteringRolls = value; OnPropertyChanged(nameof(FinishedEnteringRolls)); }
        }

        private bool _scoringFinished;
        public bool ScoringFinished
        {
            get { return _scoringFinished; }
            set { _scoringFinished = value; OnPropertyChanged(nameof(ScoringFinished)); }
        }
    }
}
