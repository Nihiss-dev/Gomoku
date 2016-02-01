using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stones.Model;

namespace Stones {
    public partial class Form1 : Form {
        enum mode
        {
            AIVAI,
            PVAI,
            PVP
        }

        const int BoardSize = 19;
        const int CellSize = 26;
        const int GameSize = 5;
         CellPlayer UserColor = CellPlayer.White;
        mode gameMode = mode.AIVAI;

        Map Board;
        AI AI;

        public Form1() {
            InitializeComponent();

            //SetClientSizeCore(CellSize * BoardSize, CellSize * BoardSize);
            SetClientSizeCore(527, 600);
            Board = new Map(BoardSize);
            AI = new AICore(Board, GameSize);

        }

        // Drawing

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            DrawBoard(e.Graphics);
        }

        void DrawBoard(Graphics g) {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach(Point location in Board.getCells()) {
                //DrawCell(g, location);
                if(Board.GetStone(location) == CellPlayer.White)
                    DrawWhiteStone(g, location);
                if(Board.GetStone(location) == CellPlayer.Black)
                    DrawBlackStone(g, location);
            }
            
        }

        void DrawCell(Graphics g, Point location) {
            Rectangle rect = CreateCellRect(location);
            g.FillRectangle(Brushes.BlueViolet, rect);
            g.DrawRectangle(Pens.Silver, rect);
        }

        void DrawWhiteStone(Graphics g, Point location) {
            Rectangle rect = CreateStoneRect(location);
            g.FillEllipse(Brushes.White, rect);
            g.DrawEllipse(Pens.Black, rect);
        }

        void DrawBlackStone(Graphics g, Point location) {
            g.FillEllipse(Brushes.Black, CreateStoneRect(location));
        }

        Rectangle CreateCellRect(Point location) {
            return new Rectangle((location.X * CellSize) + 1, (location.Y * CellSize) + 1, CellSize, CellSize);
        }

        Rectangle CreateStoneRect(Point location) {
            Rectangle result = CreateCellRect(location);
            result.Inflate(-3, -3);
            return result;
        }

        // Action

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);
            CellPlayer opponentColor;
            Point opponentLocation;

            if (e.Button == MouseButtons.Left && gameMode == mode.PVAI)
            {
                Point cellCoords = new Point(e.X / CellSize, e.Y / CellSize);
                if (Board.GetStone(cellCoords) == CellPlayer.None)
                {
                    MakeMove(cellCoords, UserColor);

                    opponentColor = AI.GetOpponentColor(UserColor);
                    opponentLocation = AI.getPlay(opponentColor);
                    if (opponentLocation == AI.Draw)
                    {
                        MessageBox.Show("Draw!");
                        Board.Clear();
                        Refresh();
                    }
                    else {
                        MakeMove(opponentLocation, opponentColor);
                    }
                }
            }
            else if (e.Button == MouseButtons.Left && gameMode == mode.AIVAI)
            {
                opponentColor = AI.GetOpponentColor(UserColor);
                opponentLocation = AI.getPlay(opponentColor);
                if (opponentLocation == AI.Draw)
                {
                    MessageBox.Show("Draw!");
                    Board.Clear();
                    Refresh();
                }
                else {
                    MakeMove(opponentLocation, opponentColor);
                }
                UserColor = (UserColor == CellPlayer.White ? CellPlayer.Black : CellPlayer.White);
            }
            else if (e.Button == MouseButtons.Left && gameMode == mode.PVP)
            {
                Point cellCoords = new Point(e.X / CellSize, e.Y / CellSize);
                if (Board.GetStone(cellCoords) == CellPlayer.None)
                {
                    MakeMove(cellCoords, UserColor);
                    UserColor = AI.GetOpponentColor(UserColor);
                }
            }
        }

        void MakeMove(Point location, CellPlayer color) {
            Board.PutStone(color, location);
            Refresh();
            if(AI.HaveVictoryAt(location, color)) {
                MessageBox.Show(color + " wins!");
                Board.Clear();
                Refresh();
            }
        }

        private void aIVsAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Board.Clear();
            Refresh();
            UserColor = CellPlayer.White;
            gameMode = mode.AIVAI;
        }

        private void playerVsIAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Board.Clear();
            Refresh();
            UserColor = CellPlayer.White;
            gameMode = mode.PVAI;
        }

        private void playerVsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Board.Clear();
            Refresh();
            UserColor = CellPlayer.White;
            gameMode = mode.PVP;
        }
    }
}
