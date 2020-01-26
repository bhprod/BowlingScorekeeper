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
            _frames = new BowlingFrame[10];
            for (int i = 0; i < 10; i++)
            {
                var bowlingFrame = new BowlingFrame(i);
                bowlingFrame.RollScoreEntered += BowlingFrame_RollScoreEntered;
                _frames[i] = bowlingFrame;
            }

            FrameTen.IsTenthFrame = true;
        }

        private ScoreKeeper _scoreKeeper;
        private void BowlingFrame_RollScoreEntered(object sender, EventArgs e)
        {
            _scoreKeeper.ScoreGame(_frames);            
        }

        #region Frames
        private BowlingFrame[] _frames;
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
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
