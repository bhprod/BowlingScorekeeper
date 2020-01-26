using BowlingScoring.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoring.ViewModel
{

    public class FrameViewModel : INotifyPropertyChanged
    {
        public FrameViewModel() 
        {
            _scoreKeeper = new ScoreKeeper();
            _currentFrameIndex = 0;
            _frames = new BowlingFrame[10];
            for (int i = 0; i < 10; i++)
            {
                var bowlingFrame = new BowlingFrame();
                bowlingFrame.RollScoreEntered += BowlingFrame_RollScoreEntered;
                _frames[i] = bowlingFrame;
            }

            FrameTen.IsTenthFrame = true;

            //FrameOne = new BowlingFrame();
            //FrameTwo = new BowlingFrame();
            //FrameThree = new BowlingFrame();
            //FrameFour = new BowlingFrame();
            //FrameFive = new BowlingFrame();
            //FrameSix = new BowlingFrame();
            //FrameSeven = new BowlingFrame();
            //FrameEight = new BowlingFrame();
            //FrameNine = new BowlingFrame();
            //FrameTen = new BowlingFrame() { IsTenthFrame = true };
        }

        private bool IsStrike(BowlingFrame frame)
        {
            return frame.FirstRollScore == NUMBER_OF_PINS;
        }

        private bool IsSpare(BowlingFrame frame)
        {
            return frame.FirstRollScore + frame.SecondRollScore == NUMBER_OF_PINS;
        }

        //this holds a max of two frames 
        private Dictionary<BowlingFrame, List<int>> _strikeFrames;

        private void BowlingFrame_RollScoreEntered(object sender, EventArgs e)
        {
                _scoreKeeper.ScoreGame(_frames);
            //if (_frameAwaitingScoring != null)
            //{
            //    _scoreKeeper.ScoreFrames(new BowlingFrame[] { _frameAwaitingScoring, _currentFrame });
            //}


            //if (_currentFrame.RollOneScore + _currentFrame.RollTwoScore == NUMBER_OF_PINS)
            //{
            //    _frameAwaitingScoring = _currentFrame;
            //}
            //else
            //{
            //    //_scoreKeeper.ScoreFrame(_currentFrame);
            //}
            //_currentFrameIndex++;

            
            //_scoreKeeper.ScoreFrames(_frames);
        }

        private int _currentFrameIndex;

        private BowlingFrame _frameAwaitingScoring;
        private BowlingFrame _strikeFrameAwaitingScoring;
        private BowlingFrame _spareFrameAwaitingScoring;

        private const int NUMBER_OF_PINS = 10;
        

        private BowlingFrame _currentFrame => _frames[_currentFrameIndex];

        private BowlingFrame[] _frames;

        private ScoreKeeper _scoreKeeper;

        public BowlingFrame FrameOne => _frames[0];
        public BowlingFrame FrameTwo => _frames[1];
        public BowlingFrame FrameThree => _frames[2];
        public BowlingFrame FrameFour => _frames[3];
        public BowlingFrame FrameFive => _frames[4];
        public BowlingFrame FrameSix => _frames[5];
        public BowlingFrame FrameSeven => _frames[6];
        public BowlingFrame FrameEight => _frames[7];
        public BowlingFrame FrameNine => _frames[8];
        public BowlingFrame FrameTen => _frames[9];

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
