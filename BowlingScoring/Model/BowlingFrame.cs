using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoring.Model
{
    /// <summary>
    /// Serves as a round in a game of bowling. A frame consists of two scores representing the two possible rolls in a round.
    /// <para/>
    /// An "x" value for the first roll signifies a strike, if a strike occurs then the second roll is skipped.
    /// A "-" value for either roll indicates a miss (no pins hit).
    /// A "/" value is a spare, which can only be rolled on the second try.
    /// <para/>
    /// This class contains minimal logic pertaining to the score of a roll.
    /// </summary>
    public class BowlingFrame : INotifyPropertyChanged
    {
        //TODO: bug where if scores are entered, then deleted in reverse order the score will not update

        public BowlingFrame() { }

        private const int STRIKE_VALUE = 10;

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
                if (value == STRIKE_VALUE) { FinishedEnteringRolls = true; }
                OnRollScoreEntered();
            }
        }
        public string FirstRollString
        {
            get { return _firstRollString; }
            set
            {
                FinishedScoring = false;

                if (string.IsNullOrEmpty(value))
                {
                    _firstRollString = "";
                    FirstRollScore = 0;
                    return;
                }

                if (value.Equals("x", StringComparison.OrdinalIgnoreCase))
                {
                    FinishedEnteringRolls = IsTenthFrame ? false : true;
                    FirstRollScore = STRIKE_VALUE;
                    SecondRollString = IsTenthFrame ? "" : "-";
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
        private int _secondRollScore;
        public int SecondRollScore
        {
            get { return _secondRollScore; }
            set { _secondRollScore = value; OnPropertyChanged(nameof(SecondRollScore)); OnRollScoreEntered(); }
        }

        private bool IsValidSpare(int compareTo)
        {
            return compareTo != STRIKE_VALUE;
        }

        //TODO group logical blocks of code together (validation next to each other)
        //write unit tests


        private string _secondRollString;
        public string SecondRollString
        {
            get { return _secondRollString; }
            set
            {
                if (value.Equals("/") && IsValidSpare(FirstRollScore)) //cannot be a spare if first roll was strike
                {
                    _secondRollString = value;
                    FinishedEnteringRolls = IsTenthFrame ? false : true;
                    SecondRollScore = STRIKE_VALUE - _firstRollScore;
                }
                else
                {
                    var parsedStringInt = ValidateStringToInt(value, _firstRollScore);
                    if (parsedStringInt == 0)
                    {
                        _secondRollString = (!IsTenthFrame && _firstRollScore == STRIKE_VALUE) ? "-" : "";
                        FinishedEnteringRolls = IsTenthFrame ? false : true;
                    }
                    else
                    {
                        _secondRollString = value;
                        FinishedEnteringRolls = IsTenthFrame ? false : true;
                    }

                    SecondRollScore = parsedStringInt;
                }
                BonusRollString = "";
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
            set
            {
                _bonusRollScore = value;
                OnRollScoreEntered();
                OnPropertyChanged(nameof(BonusRollScore));
            }
        }

        private string _bonusRollString;
        public string BonusRollString
        {
            get { return _bonusRollString; }
            set
            {
                if (value.Equals("/") && IsValidSpare(SecondRollScore))
                {
                    _bonusRollString = value;
                    FinishedEnteringRolls = IsTenthFrame ? false : true;
                    BonusRollScore = STRIKE_VALUE - _secondRollScore;
                }
                else
                {
                    var parsedStringInt = ValidateStringToInt(value, _secondRollScore);
                    if (parsedStringInt == 0)
                    {
                        _bonusRollString = (!IsTenthFrame && _firstRollScore == STRIKE_VALUE) ? "-" : value;
                        FinishedEnteringRolls = IsTenthFrame ? false : true;
                    }
                    else
                    {
                        _bonusRollString = value;
                        FinishedEnteringRolls = IsTenthFrame ? false : true;
                    }

                    BonusRollScore = parsedStringInt;
                }
                
                OnPropertyChanged(nameof(BonusRollString));
            }
        }      

        private bool _isTenthFrame;
        public bool IsTenthFrame
        {
            get { return _isTenthFrame; }
            set { _isTenthFrame = value; OnPropertyChanged(nameof(IsTenthFrame)); }
        }

        #endregion
        private int ValidateStringToInt(string value, int previousRollScore = 0)
        {
             if (value.Equals("") || value.Equals("-")) return 0;

            if (value.Equals("x", StringComparison.OrdinalIgnoreCase)) return STRIKE_VALUE;

            if (int.TryParse(value, out int toReturn))
            {
                if (toReturn < 10 && toReturn >= 0) return toReturn;
            }

            return 0;
        }


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
        public bool FinishedScoring
        {
            get { return _scoringFinished; }
            set { _scoringFinished = value; OnPropertyChanged(nameof(FinishedScoring)); }
        }
    }
}
