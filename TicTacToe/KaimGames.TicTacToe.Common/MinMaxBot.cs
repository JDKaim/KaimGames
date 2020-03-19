using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaimGames.TicTacToe.Common
{
    public class MinMaxBot : IBot
    {
        public string Name => "MinMax Bot";

        public string Description => "Uses the MinMax algorithm to guarantee it won't lose";

        public Tuple<int, int> GetMove(Game game)
        {
            MinMaxNode minMax = new MinMaxNode(game);
            return minMax.GetMove(game.IsXTurn);
        }
    }

    internal class MinMaxNode
    {
        private List<MinMaxNode> _children;
        private Game _game;
        private double _xScore;
        readonly public Tuple<int, int> Move;

        // This tracks all possible game states, which drastically improves performance. See below in the constructor
        // for an explanation as to why.
        static Dictionary<string, MinMaxNode> _nodeCache = new Dictionary<string, MinMaxNode>();

        // Cache currently disabled because it seems to return wrong data sometimes and I don't know why.
        static bool _useCache = false;

        /// <summary>
        /// The constructor calculates the score if this is a leaf node. Otherwise it builds the rest
        /// of the branch.
        /// </summary>
        /// <param name="game">The game state at this point in the path.</param>
        /// <param name="move">The move taken to get to this game state.</param>
        public MinMaxNode(Game game, Tuple<int, int> move = null)
        {
            this.Move = move;
            this._game = game;

            // If the game is over, set the scores relative to the X side. When evaluating for O later on,
            // we will just negate the scores as needed.
            if (this._game.IsXWin)
            {
                this._xScore = 1.0;
            }
            else if (this._game.IsOWin)
            {
                this._xScore = -1.0;
            }
            else if (this._game.IsTie)
            {
                this._xScore = 0.0;
            }
            else
            {
                // If we got here, it's because the game is not over. Walk the board and try each of the paths.
                this._children = new List<MinMaxNode>();

                for (int row = 0; row < 3; row++)
                {
                    for (int column = 0; column < 3; column++)
                    {
                        if (this._game.Board.IsEmptyAt(row, column))
                        {
                            // If we can move here, then copy the game, simulate the move, and add the node to the children.
                            Game copy = this._game.Copy();
                            copy.Mark(row, column);

                            // NOTE: Due to the nature of Tic-Tac-Toe, we know that all boards of the same configuration are
                            // identical. To optimize the performance, this step caches each board state by its serialized string
                            // and just reuses identical boards instead of creating new ones and iterating their paths redundantly.
                            if (MinMaxNode._useCache && MinMaxNode._nodeCache.ContainsKey(copy.Board.Serialize()))
                            {
                                this._children.Add(MinMaxNode._nodeCache[copy.Board.Serialize()]);
                            }
                            else
                            {
                                MinMaxNode node = new MinMaxNode(copy, Tuple.Create(row, column));
                                this._children.Add(node);

                                if (MinMaxNode._useCache)
                                {
                                    MinMaxNode._nodeCache.Add(copy.Board.Serialize(), node);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the best available move for the side requested.
        /// </summary>
        /// <param name="isXTurn">True if it's X's turn.</param>
        /// <returns>The move as a row, column tuple.</returns>
        public Tuple<int, int> GetMove(bool isXTurn)
        {
            // We want the best score. We know it will be at least -1, so deault to a lower value.
            double bestScore = -2.0;
            Tuple<int, int> bestMove = Tuple.Create(-1, -1);

            // Loop through the children to get the highest score. They will figure out their best min/max scores
            // internally, so we just need to call the function and assume the best score is the best move.
            foreach (MinMaxNode node in this._children)
            {
                double moveScore = node.GetScore(isXTurn);
                if (moveScore > bestScore)
                {
                    bestScore = moveScore;
                    bestMove = node.Move;
                }
            }

            return bestMove;
        }

        /// <summary>
        /// Calculates the score of this path assuming each side will take it's best available move.
        /// </summary>
        /// <param name="findingXTurn">True if we're evaluating this path to figure out the best X move from the root.</param>
        /// <returns>The expected score of this path.</returns>
        private double GetScore(bool findingXTurn)
        {
            if (this._game.IsOver) { return this._xScore * (findingXTurn ? 1 : -1); }

            // We need to know whether we're looking for a max or min score here. For example, if we're using min/max to find 
            // X's best move earlier, then we would want the lowest possible outcome on O's turn. Same for O, but in reverse.
            bool wantMax = (findingXTurn == this._game.IsXTurn);

            // Default the score outside the valid range so that the first result will definitely set it.
            double bestScore = 2.0 * (wantMax ? -1 : 1);

            foreach (MinMaxNode node in this._children)
            {
                double moveScore = node.GetScore(findingXTurn);
                if (wantMax && (moveScore > bestScore))
                {
                    bestScore = moveScore;
                }
                if (!wantMax && (moveScore < bestScore))
                {
                    bestScore = moveScore;
                }
            }

            return bestScore;
        }
    }
}
