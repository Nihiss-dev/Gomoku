using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stones.Model;
using System.Drawing;

namespace Stones {
    public abstract class AI {
        public readonly static Point Draw = new Point(-1, -1);

        Map board;
        int gameSize;        

        public AI(Map board, int gameSize) {
            if(board == null)
                throw new ArgumentNullException();
            if(gameSize < 2)
                throw new ArgumentException();
            this.board = board;
            this.gameSize = gameSize;            
        }

        public Map Board {
            get { return board; }
        }

        public int GameSize {
            get { return gameSize; }
        }

        public bool HaveVictoryAt(int x, int y, CellPlayer color) {
            return HaveVictoryAt(new Point(x, y), color);
        }

        public bool HaveVictoryAt(Point location, CellPlayer color) {
            return Board.GetStone(location) == color && getScore(location, color) >= GameSize - 1;
        }

        public abstract Point getPlay(CellPlayer color);

        public CellPlayer GetOpponentColor(CellPlayer myColor) {
            if(myColor == CellPlayer.White)
                return CellPlayer.Black;
            if(myColor == CellPlayer.Black)
                return CellPlayer.White;
            throw new ArgumentException();
        }

        int CountStonesInDirection(Point start, int dx, int dy, CellPlayer color) {
            int result = 0;

            for(int i = 1; i < GameSize; i++) {
                Point current = start + new Size(i * dx, i * dy);
                if(Board.GetStone(current) != color)
                    break;
                result++;
                Console.WriteLine("result" + result);
            }

            return result;
        }

        protected int getScore(Point location, CellPlayer color) {
            int[] counts = new int[] { 
                CountStonesInDirection(location, -1, 0, color) + CountStonesInDirection(location, 1, 0, color),
                CountStonesInDirection(location, 0, -1, color) + CountStonesInDirection(location, 0, 1, color),
                CountStonesInDirection(location, -1, -1, color) + CountStonesInDirection(location, 1, 1, color),
                CountStonesInDirection(location, -1, 1, color) + CountStonesInDirection(location, 1, -1, color)
            };

            int result = 0;
            for(int i = 0; i < counts.Length; i++) {
                result = Math.Max(result, counts[i]);
            }
            return result;
        }
    }
}
