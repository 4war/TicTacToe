
using System.Collections.Generic;
using System.Drawing;
using TicTacToe.Properties;

namespace TicTacToe
{
    public enum Sign
    {
        Empty,
        Cross,
        Circle,
    }

    public static class SignController
    {
        public static Dictionary<Sign, Image> Images = new Dictionary<Sign, Image>()
        {
            [Sign.Empty] = Resources.toe,
            [Sign.Cross] = Resources.Cross,
            [Sign.Circle] = Resources.Circle,
        };
    }
}


