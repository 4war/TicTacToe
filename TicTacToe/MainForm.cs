using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe.Properties;

namespace TicTacToe
{
    public partial class MainForm : Form
    {
        public const int PanelHeight = 600;
        public const int PanelWidth = 600;
        public const int ControlsWidth = 200;
        public const int ButtonHeight = 50;
        
        private readonly GameController _gameController;

        public readonly Panel GamePanel = new Panel()
        {
            Width = PanelWidth,
            Height = PanelHeight,
            Anchor = AnchorStyles.Left,
            Location = new Point(0, 0),
            BackColor = Defaults.DarkBlueDark,
        };

        public readonly Panel ControlPanel = new Panel()
        {
            Width = ControlsWidth,
            Height = PanelHeight,
            Anchor = AnchorStyles.Left,
            Location = new Point(PanelWidth, 0),
            BackColor = Defaults.DarkBlueLight
        };

        public readonly Button StartButton = new Button()
        {
            Text = "RESET",
            Font = Defaults.Font,
            Padding = new Padding(3),
            Margin = new Padding(3),
            FlatStyle = FlatStyle.Flat,
            Height = 50,

            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Defaults.White,
            BackColor = Defaults.DarkBlueLight,

            Anchor = AnchorStyles.Top,
            Dock = DockStyle.Top,
        };

        public readonly Label ResultLabel = new Label()
        {
            Text = "",
            Font = Defaults.Font,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Defaults.White,

            Anchor = AnchorStyles.Top,
            Dock = DockStyle.Top,
        };

        public readonly Panel ResultPanel = new Panel()
        {
            Anchor = AnchorStyles.Top,
            Dock = DockStyle.Top,

            BackColor = Defaults.DarkBlueDark,
            Height = 30,
        };

        //todo: add panel for cross and circle

        public readonly Button CrossButton = new Button()
        {
            FlatStyle = FlatStyle.Flat,
            BackColor = Defaults.DarkBlueDark,
            BackgroundImage = Resources.ChosenCross,
            BackgroundImageLayout = ImageLayout.Stretch,
            Width = 80,
            Height = 80,
            Location = new Point(){X = 10, Y = 10},
        };
        
        public readonly Button CircleButton = new Button()
        {
            FlatStyle = FlatStyle.Flat,
            BackColor = Defaults.DarkBlueLight,
            BackgroundImage = Resources.Circle,
            BackgroundImageLayout = ImageLayout.Stretch,
            Width = 80,
            Height = 80,
            Location = new Point(){X = 110, Y = 10},
        };
        
        public readonly Panel ChooseSidePanel = new Panel()
        {
            Anchor = AnchorStyles.Top,
            Dock = DockStyle.Top,

            BackColor = Defaults.DarkBlueLight,
            Height = 100
        };

        public MainForm()
        {
            InitializeComponent();

            ResultPanel.Controls.Add(ResultLabel);
            
            ChooseSidePanel.Controls.Add(CrossButton);
            ChooseSidePanel.Controls.Add(CircleButton);
            
            ControlPanel.Controls.Add(ChooseSidePanel);
            ControlPanel.Controls.Add(ResultPanel);
            ControlPanel.Controls.Add(StartButton);

            Controls.Add(GamePanel);
            Controls.Add(ControlPanel);

            _gameController = new GameController(this);
            CrossButton.Click += (sender, args) =>
            {
                CrossButton.BackgroundImage = Resources.ChosenCross;
                CircleButton.BackgroundImage = Resources.Circle;
                _gameController.Reset(Sign.Cross);
            };
            
            CircleButton.Click += (sender, args) =>
            {
                CircleButton.BackgroundImage = Resources.ChosenCircle;
                CrossButton.BackgroundImage = Resources.Cross;
                _gameController.Reset(Sign.Circle);
            };
            
            _gameController.Reset(Sign.Cross);
        }
    }
}