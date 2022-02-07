using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToe.AI
{
    public class AILogic
    {
        private Random _random = new Random();

        public Move MakeMove(GameController game, Sign aiSign)
        {
            //return DecideRandomMove(game, aiSign);
            return DecideCorrectMove(game, aiSign);
        }

        private Move DecideRandomMove(GameController game, Sign aiSign)
        {
            var randomList = game.GetEmptyCells()
                .Select(c => new Move() { X = c.X, Y = c.Y, Sign = aiSign })
                .ToList();

            var randomMove = randomList[_random.Next(randomList.Count)];
            return randomMove;
        }

        private Move DecideCorrectMove(GameController game, Sign aiSign)
        {
            if (game.Field[1,1] is Sign.Empty)
            {
                return new Move() { X = 1, Y = 1, Sign = aiSign };
            }

            if (game.GetEmptyCells().Count == 8)
            {
                var possibleMoves = new List<Move>()
                {
                    new Move() { X = 0, Y = 0, Sign = aiSign },
                    new Move() { X = 2, Y = 0, Sign = aiSign },
                    new Move() { X = 2, Y = 2, Sign = aiSign },
                    new Move() { X = 0, Y = 2, Sign = aiSign },
                };
                return possibleMoves[_random.Next(possibleMoves.Count)];
            }
            
            return WidthSearch(game, aiSign);
        }

        private Move WidthSearch(GameController game, Sign aiSign)
        {
            var queue = new Queue<Node>();
            var startNode = new Node(game.Field);
            queue.Enqueue(startNode);
            var visited = new HashSet<Node>();
            var theDeepestNode = startNode;

            for (; ;)
            {
                if (!queue.Any())
                {
                    return GetPrimalMove(theDeepestNode);
                }
                
                var node = queue.Dequeue();
                if (visited.Contains(node))
                {
                    continue;
                }
                visited.Add(node);

                var nextNodes = game.GetEmptyCells(node.Field)
                    .Select(c => new Move() { X = c.X, Y = c.Y, Sign = GetCurrentSign(!node.isAI, aiSign) })
                    .Select(move => new Node(GetFieldAfterMove(node.Field, move)) 
                        { 
                            Move = move, 
                            Depth = node.Depth + 1, 
                            isAI = !node.isAI, 
                            Parent = node} ).ToList();
                
                foreach (var nextNode in nextNodes)
                {
                    if (visited.Contains(nextNode))
                    {
                        continue;
                    }
                    
                    if (game.CheckAllWinConditions(GetCurrentSign(nextNode.isAI, aiSign), nextNode.Field))
                    {
                        if (nextNode.isAI)
                        {
                            return GetPrimalMove(nextNode);
                        }
                        else 
                        {
                            foreach (var next in nextNodes)
                            {
                                visited.Add(next);
                            }
                            break;
                        }
                    }

                    if (nextNode.Depth > theDeepestNode.Depth)
                        theDeepestNode = nextNode;
                    queue.Enqueue(nextNode);
                }
            }
        }

        private Move GetPrimalMove(Node node)
        {
            var result = node;
            for (; result.Depth > 1; result = result.Parent){}
            return result.Move;
        }
        
        private Sign GetCurrentSign(bool isAi, Sign aiSign)
        {
            if (isAi)
            {
                return aiSign == Sign.Circle ? Sign.Circle : Sign.Cross;
            }
            else
            {
                return aiSign == Sign.Circle ? Sign.Cross : Sign.Circle;
            }
        }

        private Sign[,] GetFieldAfterMove(Sign[,] currentField, Move move)
        {
            var resultField = new Sign[currentField.GetLength(0), currentField.GetLength(1)];

            for (var x = 0; x < currentField.GetLength(0); x++)
            for (var y = 0; y < currentField.GetLength(1); y++)
                resultField[x, y] = currentField[x, y];

            resultField[move.X, move.Y] = move.Sign;
            return resultField;
        }
    }
}