using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stones.Model {
    public class Map {
        int size;
        CellPlayer[,] stones;

        public Map(int size) {
            if(size < 1)
                throw new ArgumentException();

            this.size = size;
            Clear();
        }

        public int Size {
            get { return size; }
        }

        public void PutStone(CellPlayer color, Point location) {
            PutStone(color, location.X, location.Y);
        }

        public void PutStone(CellPlayer color, int x, int y) {
            if(IsOutside(x, y))
                throw new ArgumentException();
            this.stones[x, y] = color;
        }

        public CellPlayer GetStone(Point location) {
            return GetStone(location.X, location.Y);
        }

        public CellPlayer GetStone(int x, int y) {
            if(IsOutside(x, y))
                return CellPlayer.None;
            return this.stones[x, y];
        }

        public bool IsOutside(int x, int y) {
            return x < 0 || x > Size - 1 || y < 0 || y > Size - 1;
        }

        public IEnumerable<Point> getCells() {
            for(int y = 0; y < Size; y++) {
                for(int x = 0; x < Size; x++) {
                    yield return new Point(x, y);
                }
            }
        }

        public void Clear() {
            this.stones = new CellPlayer[size, size];
        }
    }
}
