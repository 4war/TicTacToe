namespace TicTacToe.AI
{
    public class AIPlayer : IPlayer
    {
        private readonly AILogic _logic = new AILogic();
        
        public Sign Own { get; }
        public AIPlayer(Sign own)
        {
            Own = own;
        }
        
        public Move MakeMove(GameController game)
        {
            return _logic.MakeMove(game, Own);
        }
    }
}