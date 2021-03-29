using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using DummyChess.Domain;

namespace DummyChess
{
    public partial class MainWindow : Window
    {
        public Game Game { get; }

        public MainWindow()
        {
            
            Game = new Game();
            DataContext = Game;
            InitializeComponent();
            //InitBoard();
        }

        private void InitBoard()
        {
            for (var i = 0; i < 8; i++)
            {
                GridBoard.ColumnDefinitions.Add(new ColumnDefinition());
                GridBoard.RowDefinitions.Add(new RowDefinition());

                for (var j = 0; j < 8; j++)
                {
                    var cellColor = i % 2 == 0
                        ? (j % 2 == 0 ? Colors.White : Colors.Black)
                        : (j % 2 == 0 ? Colors.Black : Colors.White);

                    var cell = new Rectangle
                    {
                        Fill = new SolidColorBrush(cellColor),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    
                    GridBoard.Children.Add(cell);
                    Grid.SetColumn(cell, i);
                    Grid.SetRow(cell, j);
                }
            }
        }
    }
}
