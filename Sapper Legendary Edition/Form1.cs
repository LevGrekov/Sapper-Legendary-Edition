using System.Security.Cryptography.X509Certificates;
using static System.Reflection.Metadata.BlobBuilder;

namespace Sapper_Legendary_Edition
{
    public partial class Form1 : Form
    {
        private const int mapsize = 20;
        private const int CellSize = 43;
        public Cell[,] cells = new Cell[mapsize, mapsize];
        private const int bombsAmaunt = 50 ;
        private bool TheFirstMove = true;
        private Point FirstClick;
        int marks = 0;
        private int truemarks = 0;
        private int opened = 0;
        public Form1()
        {
            this.Width = mapsize * CellSize + 20;
            this.Height = (mapsize + 1) * (CellSize + 1);
            this.Text = "Sapper";
            generatemap();
        }
        private void generatemap()
        {
            for (int i = 0; i < (mapsize); i++)
            {
                for (int j = 0; j < (mapsize); j++)
                {
                    var cell = new Cell();
                    cell.Location = new Point(CellSize * i, CellSize * j);
                    cell.Size = new Size(CellSize, CellSize);
                    this.Controls.Add(cell);
                    cells[i, j] = cell;
                    cells[i, j].MouseUp += new MouseEventHandler(OnButtonPressedMouse);
                    cell.Image = Properties.Resources.empty;
                }
            }
        }
        private void OnButtonPressedMouse(object? sender, MouseEventArgs e)
        {
            var Cell = sender as Cell;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    LeftClick(Cell);
                    break;
                case MouseButtons.Right:
                    RightClick(Cell);
                    break;
            }
        }
        private void LeftClick(Cell Cell)
        {
            
            int ibutton = Cell.Location.X / CellSize;
            int jbutton = Cell.Location.Y / CellSize;
            if (cells[ibutton, jbutton].Marked)
            {
                return;
            }
            else
            {
                Cell.Enabled = false;
                if (TheFirstMove)
                {
                    FirstClick = new Point(Cell.Location.X / CellSize, Cell.Location.Y / CellSize);
                    generatebombs();
                    //showbombs();
                    BombsAraund();
                    TheFirstMove = false;
                }
                if (cells[ibutton, jbutton].bombsAraund == -1)
                {
                        Detonate(ibutton, jbutton);
                }
                OpenZeroCells(ibutton, jbutton);
            }
        }
        private void RightClick(Cell Cell)
        {
            if (!Cell.Marked)
            {
                Cell.Marked = true;
                if (Cell.bombsAraund == -1)
                {
                    marks++;
                    truemarks++;
                    Cell.Image = Properties.Resources.flag;
                }
                else
                {
                    marks++;
                    Cell.Image = Properties.Resources.flag;
                }
            }
            else
            {
                Cell.Marked = false;
                if (Cell.bombsAraund == -1)
                {
                    marks--;
                    truemarks--;
                    Cell.Image = Properties.Resources.empty;
                }
                else
                {
                    marks--;
                    Cell.Image = Properties.Resources.empty;
                }
            }
            _markscheck();
        }
        private void _showbombs()
        {
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (cells[i, j].bombsAraund == -1)
                    {
                        cells[i, j].BackColor = Color.Red;
                    }
                }
            }
        }
        private void BombsAraund()
        {
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (cells[i, j].bombsAraund == -1)
                    {
                        for (int ni = -1; ni <= 1; ni++)
                        {
                            for (int nj = -1; nj <= 1; nj++)
                            {
                                if (i + ni < 0 || j + nj < 0 || i + ni > mapsize - 1 || j + nj > mapsize - 1 || cells[i + ni, j + nj].bombsAraund == -1) continue;
                                cells[i + ni, j + nj].bombsAraund++;
                            }
                        }
                    }
                }
            }
        }
        private void generatebombs()
        {
            var r = new Random();
            int rI, rJ;
            for (int k = 0; k < bombsAmaunt; k++)
            {
                do
                {
                    rI = r.Next(0, mapsize);
                    rJ = r.Next(0, mapsize);
                }
                while (cells[rI, rJ].bombsAraund == -1 || (Math.Abs(rI - FirstClick.X) <= 1 && Math.Abs(rJ - FirstClick.Y) <= 1));
                cells[rI, rJ].bombsAraund = -1;
            }
        }
        private void OpenCell(int i, int j)
        {
            cells[i, j].Enabled = false;
            opened++;
            //if (map[i, j] == -10) return;
            switch (cells[i, j].bombsAraund)
            {
                case 1:
                    cells[i, j].Image = Properties.Resources._1;
                    break;
                case 2:
                    cells[i, j].Image = Properties.Resources._2;
                    break;
                case 3:
                    cells[i, j].Image = Properties.Resources._3;
                    break;
                case 4:
                    cells[i, j].Image = Properties.Resources._4;
                    break;
                case 5:
                    cells[i, j].Image = Properties.Resources._5;
                    break;
                case 6:
                    cells[i, j].Image = Properties.Resources._6;
                    break;
                case 7:
                    cells[i, j].Image = Properties.Resources._7;
                    break;
                case 8:
                    cells[i, j].Image = Properties.Resources._8;
                    break;
                case 0:
                    cells[i, j].Image = Properties.Resources._0;
                    break;
            }
            _markscheck();
        }
        private void OpenZeroCells(int i, int j)
        {
            OpenCell(i, j);
            if (cells[i, j].bombsAraund > 0) return;

            for (int ni = -1; ni <= 1; ni++)
            {
                for (int nj = -1; nj <= 1; nj++)
                {
                    if (ni == 0 && nj == 0) continue;
                    if (i + ni < 0 || j + nj < 0) continue;
                    if (i + ni > mapsize - 1 || j + nj > mapsize - 1) continue;
                    if (!cells[i + ni, j + nj].Enabled) continue;
                    if (cells[i + ni, j + nj].bombsAraund == 0) OpenZeroCells(i + ni, j + nj);
                    if (cells[i + ni, j + nj].bombsAraund > 0) OpenCell(i + ni, j + nj);
                }
            }
        }
        private void Detonate(int n, int m)
        {
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (cells[i, j].bombsAraund == -1)
                    {
                        if (cells[i, j].Marked) continue;
                        else
                        {
                            cells[i, j].Image = cells[i, j] != cells[n, m] ? Properties.Resources.bomb : Properties.Resources.detonated;
                        }
                    }
                    else if (cells[i,j].Marked && cells[i,j].bombsAraund != -1)
                    {
                        cells[i, j].Image = Properties.Resources.Dflag;
                    }
                }
            }
            if (MessageBox.Show($"Бабах", "Вы Проиграли") == DialogResult.OK)
            {
                this.Close();
            }
        }
        private void _markscheck()
        {
            if (truemarks == bombsAmaunt && marks == truemarks && opened + truemarks == mapsize*mapsize)
            {
                if (MessageBox.Show("Вы Победили") == DialogResult.OK)
                {
                    this.Close();
                }
            }
            return;
        }
    }
}