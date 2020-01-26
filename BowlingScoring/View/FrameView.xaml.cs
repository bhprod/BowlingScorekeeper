using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BowlingScoring.View
{
    /// <summary>
    /// Interaction logic for FrameView.xaml
    /// </summary>
    public partial class FrameView : UserControl
    {
        public FrameView()
        {
            InitializeComponent();
        }

        //public DependencyProperty IsTenthFrameProperty = DependencyProperty.Register("IsTenthFrame", typeof(bool), typeof(FrameView), null);

        //public bool IsTenthFrame
        //{
        //    get => (bool)GetValue(IsTenthFrameProperty);
        //    set => SetValue(IsTenthFrameProperty, value);
        //}
    }
}
