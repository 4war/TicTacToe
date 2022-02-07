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
        public bool isAI { get; set; }
        
        public Node Parent { get; set; }
    }
}