using System.Collections.Generic;
using System.Drawing;

namespace TicTacToe
{
    public static class Defaults
    {
        public static readonly Color DarkBlueDark = ColorTranslator.FromHtml("#0e1722"); 
        public static readonly Color DarkBlueLight = ColorTranslator.FromHtml("#141f31"); 
        public static readonly Color White = ColorTranslator.FromHtml("#e1e3e6");

        public static readonly Font Font = new Font("Arial", 14);

        public static readonly Dictionary<Result, string> Messages = new Dictionary<Result, string>()
        {
            [Result.None] = "",
            [Result.JustStarted] = "",
            [Result.Draw] = "Draw",
            [Result.WinCircle] = "Circle Win",
            [Result.WinCross] = "Cross Win",
        };
    }
}