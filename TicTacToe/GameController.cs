using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TicTacToe.AI;

namespace TicTacToe
{
    public enum Result
    {
        None,
        WinCross,
        WinCircle,
        Draw,
        JustStarted,
    }

    public class GameController
    {
        private readonly Drawer _drawer;
        private const int HorizontalCells = 3;
        private const int VerticalCells = 3;

        private HumanPlayer _humanPlayer;
        private AIPlayer _aiPlayer;
        private MainForm _form;
        private Result _result;

        private Timer _timer = new Timer();
        private Random _random = new Random();


        public Sign[,] Field { get; set; }

        public Sign ChosenSign { get; set; }

        public Result Result
        {
            get => _result;
            set
            {
                _result = value;
                _form.ResultLabel.Text = Defaults.Messages[_result];
                _form.CircleButton.Enabled = Result != Result.None;
                _form.CrossButton.Enabled = Result != Result.None;
            }
        }

        public IPlayer CurrentPlayer { get; set; }

        public bool Blocked => CurrentPlayer is AIPlayer ||
                               Result != Result.None && !(CurrentPlayer is HumanPlayer && Result == Result.JustStarted);

        public GameController(MainForm form)
        {
            _form = form;
            Field = new Sign[HorizontalCells, VerticalCells];
            _drawer = new Drawer(_form.GamePanel, this);
            _drawer.DrawField(HorizontalCells, VerticalCells);
            _form.StartButton.Click += (sender, args) => Reset(ChosenSign);
        }

        public void Reset(Sign signForHuman)
        {
            _drawer.Reset(HorizontalCells, VerticalCells);
            Result = Result.JustStarted;
            Field = new Sign[HorizontalCells, VerticalCells];

            UpdateChosenSide(signForHuman);
        }

        public void Confirm(int x, int y) => MakeMove(new Move() { X = x, Y = y, Sign = CurrentPlayer.Own });

        public void MakeMove(Move move)
        {
            _drawer.DrawSign(move);
            Field[move.X, move.Y] = CurrentPlayer.Own;
            UpdateResult(move.Sign);
            if (Result == Result.None)
            {
                SwitchMove();
            }
        }

        public void UpdateChosenSide(Sign sign)
        {
            ChosenSign = sign;
            _humanPlayer = new HumanPlayer(sign);
            _aiPlayer = new AIPlayer(sign == Sign.Circle ? Sign.Cross : Sign.Circle);
            CurrentPlayer = sign == Sign.Circle ? (IPlayer)_aiPlayer : (IPlayer)_humanPlayer;

            if (CurrentPlayer is AIPlayer)
            {
                WaitForAIToMove();
                Result = Result.JustStarted;
            }
        }

        public List<Cell> GetEmptyCells(Sign[,] field)
        {
            var list = new List<Cell>();
            for (var x = 0; x < field.GetLength(0); x++)
            for (var y = 0; y < field.GetLength(1); y++)
                if (field[x, y] is Sign.Empty)
                    list.Add(new Cell() { X = x, Y = y });

            return list;
        }

        public List<Cell> GetEmptyCells()
        {
            var list = new List<Cell>();
            for (var x = 0; x < Field.GetLength(0); x++)
            for (var y = 0; y < Field.GetLength(1); y++)
                if (Field[x, y] is Sign.Empty)
                    list.Add(new Cell() { X = x, Y = y });

            return list;
        }

        public void UpdateResult(Sign sign)
        {
            var emptyCells = GetEmptyCells();
            if (emptyCells.Count == HorizontalCells * VerticalCells)
            {
                Result = Result.JustStarted;
                return;
            }

            Result = CheckAllWinConditions(sign, Field)
                ? sign == Sign.Circle
                    ? Result.WinCircle
                    : Result.WinCross
                : emptyCells.Any()
                    ? Result.None
                    : Result.Draw;
        }


        public bool CheckAllWinConditions(Sign mark, Sign[,] field)
        {
            var diagonalTopLeft = CheckDiagonalWinCondition(
                (x, y) => field[x, y] != mark && x == y);

            var diagonalTopRight = CheckDiagonalWinCondition(
                (x, y) => field[x, y] != mark && x == field.GetLength(1) - y - 1);

            var horizontal = CheckSideWinCondition(field, (x, y) => field[y, x] != mark);
            var vertical = CheckSideWinCondition(field, (x, y) => field[x, y] != mark);

            return diagonalTopLeft || diagonalTopRight || horizontal || vertical;
        }

        private bool CheckDiagonalWinCondition(Func<int, int, bool> condition)
        {
            for (var y = 0; y < Field.GetLength(0); y++)
            for (var x = 0; x < Field.GetLength(1); x++)
                if (condition(x, y))
                    return false;

            return true;
        }

        private bool CheckSideWinCondition(Sign[,] field, Func<int, int, bool> condition)
        {
            for (var y = 0; y < field.GetLength(0); y++)
            {
                var tempResult = true;
                for (var x = 0; x < field.GetLength(1); x++)
                    if (condition(x, y))
                        tempResult = false;

                if (tempResult)
                    return true;
            }

            return false;
        }

        private void SwitchMove()
        {
            CurrentPlayer = CurrentPlayer is AIPlayer ? (IPlayer)_humanPlayer : _aiPlayer;
            if (GetEmptyCells().Any())
                if (CurrentPlayer is AIPlayer)
                    WaitForAIToMove();
        }

        private void WaitForAIToMove()
        {
            Wait(200);
            var aiMove = _aiPlayer.MakeMove(this);
            MakeMove(aiMove);
        }

        private void Wait(int milliseconds)
        {
            if (milliseconds == 0 || milliseconds < 0) return;

            _timer.Start();
            _timer.Interval = milliseconds;
            _timer.Enabled = true;

            _timer.Tick += (s, e) =>
            {
                _timer.Enabled = false;
                _timer.Stop();
            };

            for (; _timer.Enabled;)
            {
                Application.DoEvents();
            }
        }
    }
}