using System;
using System.Drawing;
using System.Windows.Forms;
using TicTacToe.Properties;

namespace TicTacToe
{
    public class MyButton : Panel
    {
        private GameController _gameController;
        
        public override Color BackColor { get; set; } = Defaults.DarkBlueDark;
        public FlatStyle FlatStyle { get; set; } = FlatStyle.Flat;
        public int X { get; set; }
        public int Y { get; set; }
        public Sign Sign { get; set; } = Sign.Empty;

        public MyButton(GameController gameController)
        {
            _gameController = gameController;
            BorderStyle = BorderStyle.FixedSingle;
            
            Click += (sender, args) =>
            {
                if (Sign != Sign.Empty || _gameController.Blocked)
                    return;
                
                _gameController.Confirm(X, Y);
            };
            
            MouseEnter += (sender, args) =>
            {
                if (Sign != Sign.Empty || _gameController.Blocked)
                    return;
                
                BackgroundImage = Resources.can_be_pressed;
            };
            
            MouseLeave += (sender, args) =>
            {
                if (Sign != Sign.Empty || _gameController.Blocked)
                    return;
                
                BackgroundImage = Resources.toe;
            };
        }

        public void SetSign(Sign sign)
        {
            Sign = sign;
            BackgroundImage = SignController.Images[sign];
        }
    }
}