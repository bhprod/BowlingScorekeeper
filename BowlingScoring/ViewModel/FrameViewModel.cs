using BowlingScoring.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoring.ViewModel
{
    /// <summary>
    /// Serves to bridge the gap between the FrameView and a BowlingFrame (the model).
    /// Subscribes to a BowlingFrame's score updates and then calls ScoreKeeper to calculate the frame's score.
    /// </summary>
    public class FrameViewModel
    {
        public FrameViewModel() {

            _frames = new BowlingFrame[10];
            for (int i = 0; i < 10; i++)
            {
                var bowlingFrame = new BowlingFrame();
                bowlingFrame.RollScoreEntered += BowlingFrame_RollScoreEntered;
                _frames[i] = bowlingFrame;
            }
            FrameTen.IsTenthFrame = true;
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

        private void BowlingFrame_RollScoreEntered(object sender, EventArgs e)
        {
            new ScoreKeeper().ScoreGame(_frames);            
        }        
    }
}
