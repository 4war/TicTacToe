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
            return DecideCorrectMove(game, aiSign);
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
            
            return PointSearch(game, aiSign);
        }

        private Move PointSearch(GameController game, Sign aiSign)
        {
            var startNode = new Node(game.Field) { Points = 0 };
            var depthDictionary = new Dictionary<int, List<Node>>();
            var queue = new Queue<Node>();
            queue.Enqueue(startNode);

            for (;;)
            {
                if (!queue.Any())
                {
                    return GetBestMove(depthDictionary);
                }
                
                var node = queue.Dequeue();
                
                if (!depthDictionary.ContainsKey(node.Depth))
                {
                    depthDictionary[node.Depth] = new List<Node>();
                }
                
                depthDictionary[node.Depth].Add(node);

                var nextNodes = game.GetEmptyCells(node.Field)
                    .Select(c => new Move() { X = c.X, Y = c.Y, Sign = GetCurrentSign(!node.AIMove, aiSign) })
                    .Select(move => new Node(GetFieldAfterMove(node.Field, move)) 
                    { 
                        Move = move, 
                        Depth = node.Depth + 1, 
                        AIMove = !node.AIMove, 
                        Parent = node} ).ToList();
                
                foreach (var nextNode in nextNodes)
                {
                    node.Children.Add(nextNode);
                    nextNode.Parent = node;
                    if (game.CheckAllWinConditions(GetCurrentSign(nextNode.AIMove, aiSign), nextNode.Field))
                    {
                        nextNode.Points = nextNode.AIMove ? 10 : -10;
                    }
                    else
                    {
                        queue.Enqueue(nextNode);
                    }
                }
            }
        }

        private Move GetBestMove(Dictionary<int, List<Node>> depthDictionary)
        {
            foreach (var kvp in depthDictionary.Skip(1).Reverse().Skip(1))
            {
                foreach (var node in kvp.Value)
                {
                    node.CalculatePoints();
                }
            }

            var max = depthDictionary[0].First().Children.Max(x => x.Points);
            var result = depthDictionary[0].First().Children.First(x => x.Points == max);
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