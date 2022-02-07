using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.AI
{
    public class Node
    {
        public Node(Sign[,] field)
        {
            Field = field;
        }

        public Move Move { get; set; }
        public Sign[,] Field { get; }
        public int Depth { get; set; }
        public bool AIMove { get; set; }

        public int Points { get; set; }

        public Node Parent { get; set; }

        public List<Node> Children { get; } = new List<Node>();

        public void CalculatePoints()
        {
            if (Children.Count == 0)
            {
                return;
            }
            
            if (!AIMove)
            {
                Points = Children.Max(x => x.Points);
            }
            else
            {
                Points = Children.Min(x => x.Points);
            }
        }
    }
}