using System.Drawing;
using System.Windows.Forms;
using TicTacToe.Properties;

namespace TicTacToe
{
    public class Drawer
    {
        private Field _field = new Field();
        private readonly Panel _panel;
        private readonly GameController _gameController;
            
        private readonly TableLayoutPanel _table = new TableLayoutPanel()
        {
            Height = MainForm.PanelHeight,
            Width = MainForm.PanelWidth
        };


        public Drawer(Panel panel, GameController gameController)
        {
            _panel = panel;
            _gameController = gameController;
            _panel.Controls.Add(_table);
        }


        public void DrawSign(Move move)
        {
            var cell = (MyButton)_table.GetControlFromPosition(move.X, move.Y);
            cell.SetSign(move.Sign);
        }

        public void Reset(int horizontalCells, int verticalCells)
        {
            //todo: reset map    
            for (var x = 0; x < horizontalCells; x++)
            {
                for (var y = 0; y < verticalCells; y++)
                {
                    var cell = (MyButton)_table.GetControlFromPosition(x, y);
                    cell.Sign = Sign.Empty;
                    cell.BackgroundImage = SignController.Images[Sign.Empty];
                }
            }
        }
        
        public void DrawField(int horizontalCells, int verticalCells)
        {
            _table.ColumnStyles.Clear();
            _table.RowStyles.Clear();
            var cellWidth = _panel.Width / horizontalCells;
            var cellHeight = _panel.Height / verticalCells;
            
            for (var i = 0; i < horizontalCells; i++)
            {
                _table.ColumnStyles.Add(new ColumnStyle()
                {
                    SizeType = SizeType.Absolute,
                    Width = cellWidth,
                });
            }

            for (var i = 0; i < verticalCells; i++)
            {
                _table.RowStyles.Add(new RowStyle()
                {
                    SizeType = SizeType.Absolute,
                    Height = cellHeight,
                });
            }

            for (var x = 0; x < horizontalCells; x++)
            {
                for (var y = 0; y < verticalCells; y++)
                {
                    var button = new MyButton(_gameController)
                    {
                        X = x,
                        Y = y,
                        Width = cellWidth,
                        Height = cellHeight,
                        BackgroundImage = Resources.toe,
                        BackgroundImageLayout = ImageLayout.Stretch,
                    };
                    _table.Controls.Add(button, x, y);
                }
            }
        }
    }

    public class Field
    {
        public int Height { get; set; } = 600;
        public int Width { get; set; } = 600;
        public Color BackgroundColor { get; } = Color.Firebrick;
    }
}