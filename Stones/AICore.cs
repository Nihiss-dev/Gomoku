using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stones.Model;
using System.Drawing;

namespace Stones {
    class AICore : AI {
        delegate IComparable Calculate(Point location, CellPlayer color);

        static readonly Random rnd = new Random();

        public AICore(Map board, int gameSize)
            : base (board, gameSize) {

        }

        public override Point getPlay(CellPlayer color) {
            var candidates = DepthThree(color);

            if (candidates.Count == 0)
                return Draw;

            if (candidates.Count == 1)
                return candidates[0];
            Console.WriteLine(candidates.Count);

            int index = rnd.Next(0, candidates.Count - 1);
            return candidates[index];
        }


        internal List<Point> DepthOne(CellPlayer color) {
            return DepthCore(Board.getCells(), CalcDepthOne, color);
        }

        List<Point> DepthTwo(CellPlayer color) {
            return DepthCore(DepthOne(color), CalcDepthTwo, color);
        }

        List<Point> DepthThree(CellPlayer color) {
            return DepthCore(DepthTwo(color), CalcDepthThree, color);
        }

        List<Point> DepthCore(IEnumerable<Point> source, Calculate calculator, CellPlayer color) {
            var result = new List<Point>();
            IComparable bestEstimate = null;

            foreach (Point location in source) {
                if (Board.GetStone(location) != CellPlayer.None)
                    continue;
                var calc = calculator(location, color);

                int compareResult = calc.CompareTo(bestEstimate);
                if (compareResult < 0)
                    continue;
                if (compareResult > 0) {
                    result.Clear();
                    bestEstimate = calc;
                }
                result.Add(location);
            }

            return result;
        }


        internal IComparable CalcDepthOne(Point location, CellPlayer color) {
            int selfScore = 1 + getScore(location, color);
            int opponentScore = 1 + getScore(location, GetOpponentColor(color));

            if(selfScore >= GameSize)
                selfScore = int.MaxValue;

            return Math.Max(selfScore, opponentScore);
        }

        internal IComparable CalcDepthOne(int x, int y, CellPlayer color) {
            return CalcDepthOne(new Point(x, y), color);
        }

        internal IComparable CalcDepthTwo(Point location, CellPlayer color) {
            int cx = location.X;
            int cy = location.Y;

            int selfCount = 0;
            int opponentCount = 0;

            for (int x = cx - 1; x <= cx + 1; x++) {
                for (int y = cy - 1; y <= cy + 1; y++) {
                    if (Board.GetStone(x, y) == color)
                        selfCount++;
                    if (Board.GetStone(x, y) == GetOpponentColor(color))
                        opponentCount++;
                }
            }
            Console.WriteLine("stage 2 " + (2 * selfCount + opponentCount));
            return 2 * selfCount + opponentCount;
        }

        internal IComparable CalcDepthTwo(int x, int y, CellPlayer color) {
            return CalcDepthTwo(new Point(x, y), color);
        }

        internal IComparable CalcDepthThree(Point location, CellPlayer color) {
            var dx = location.X - Board.Size / 2;
            var dy = location.Y - Board.Size / 2;
            return -Math.Sqrt(dx * dx + dy * dy);
        }

        internal IComparable CalcDepthThree(int x, int y, CellPlayer color) {
            return CalcDepthThree(new Point(x, y), color);
        }
    }
}
