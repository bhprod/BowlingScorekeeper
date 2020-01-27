using BowlingScoring.Model;
using BowlingScoring.ViewModel;
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
    /// Interaction logic for GameBoardView.xaml
    /// </summary>
    public partial class GameBoardView : UserControl
    {
        public GameBoardView()
        {
            _scoreKeeper = new ScoreKeeper();
            InitializeComponent();
        }

        private ScoreKeeper _scoreKeeper;
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            var gameBoard = FindName("GameBoard") as StackPanel;

            gameBoard.Children.Add(new SingleGameView());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var gameBoard = FindName("GameBoard") as StackPanel;
            var currentGames = new List<SingleGameView>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gameBoard); i++)
            {
                var childElement = (Visual)VisualTreeHelper.GetChild(gameBoard, i);

                if (childElement.GetType() == typeof(SingleGameView))
                {
                    currentGames.Add(childElement as SingleGameView);
                }
            }

            foreach (var game in currentGames)
            {
                gameBoard.Children.Remove(game);
            }
            gameBoard.Children.Add(new SingleGameView() { DataContext = new FrameViewModel()});
        }
    }
}
