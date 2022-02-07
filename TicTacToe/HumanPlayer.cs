namespace TicTacToe
{
    public class HumanPlayer : IPlayer
    {
        public Sign Own { get; }
        public HumanPlayer(Sign own)
        {
            Own = own;
        }
    }
}