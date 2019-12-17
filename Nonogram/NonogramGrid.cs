using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Nonogram
{
    public class NonogramGrid
    {
        public List<NonogramLine> Rows { get; set; }
        public List<NonogramLine> Columns { get; set; }
        public List<List<NonogramCell>> Cells { get; set; }

        public NonogramGrid()
        {
            Rows = new List<NonogramLine>();
            Columns = new List<NonogramLine>();
            Cells = new List<List<NonogramCell>>();
        }

        public NonogramGrid GetBoardCells(Bitmap lowResImage)
        {
            NonogramGrid grid = new NonogramGrid();
            for (int col = 0; col < lowResImage.Width; col++)
            {
                grid.Columns.Add(new NonogramLine()
                {
                    IsRow = false,
                    Location = col,
                });

                List<NonogramCell> columnCells = new List<NonogramCell>();

                for (int row = 0; row < lowResImage.Height; row++)
                {
                    if (col == 0)
                    {
                        grid.Rows.Add(new NonogramLine()
                        {
                            IsRow = true,
                            Location = row
                        });
                    }

                    Color color = lowResImage.GetPixel(col, row);
                    bool isBlackCell = color.R == 0;

                    grid.Rows[row].AddCell(col, isBlackCell);
                    grid.Columns[col].AddCell(row, isBlackCell);
                    columnCells.Add(new NonogramCell(row, col, isBlackCell));

                    // Add terminating cell for every row after final column
                    if (col == lowResImage.Width - 1)
                        grid.Rows[row].AddCell(col + 1, false);
                }

                // Add terminating cell for every column after final row
                grid.Columns[col].AddCell(0, false);
                grid.Cells.Add(columnCells);
            }

            return grid;
        }

        public Bitmap NonogramFromImage(Size origSize, Size lowResSize, NonogramGrid grid, bool showSolution)
        {
            int origWidth = origSize.Width;
            int origHeight = origSize.Height;
            int lowWidth = lowResSize.Width;
            int lowHeight = lowResSize.Height;
            int cellWidth = origWidth / lowWidth;
            int cellHeight = origHeight / lowHeight;

            int longestRow = grid.Rows.Select((x) => x.BlackGroups.Count).Max();
            int longestCol = grid.Columns.Select((x) => x.BlackGroups.Count).Max();

            int extraWidth = longestRow * cellWidth;
            int extraHeight = longestRow * cellHeight;

            int fullWidth = (origWidth - (origWidth % lowWidth)) + extraWidth;
            int fullHeight = (origHeight - (origHeight % lowHeight)) + extraHeight;

            Bitmap nonogramImage = new Bitmap(fullWidth, fullHeight);
            Graphics gr = Graphics.FromImage(nonogramImage);

            gr.DrawImage(nonogramImage, new Rectangle(0, 0, nonogramImage.Width, nonogramImage.Height));

            Pen pen = new Pen(Brushes.Black);
            Font numberFont = GetFont(cellWidth, cellHeight, gr);

            // Bold lines separating clues from image
            Rectangle leftRect = new Rectangle() 
            {
                X = (longestRow) * cellWidth - 1,
                Y = 0,
                Width = 3,
                Height = fullHeight,
            };
            Rectangle topRect = new Rectangle()
            {
                X = 0,
                Y = longestCol * cellHeight - 1,
                Width = fullWidth,
                Height = 3,
            }; 
            gr.DrawRectangles(pen, new RectangleF[] { leftRect, topRect });

            List<List<NonogramCell>> cells = grid.Cells;
            int colMax = cells.Count;
            int rowMax = cells[0].Count;
            for (int col = 0; col < colMax + longestCol; col++)
            {
                gr.DrawLine(pen, (col + 1) * cellWidth, 0, (col + 1) * cellWidth, fullHeight);
                for (int row = 0; row < rowMax + longestRow; row++)
                {
                    if (col == 0)
                        gr.DrawLine(pen, 0, (row + 1) * cellHeight, fullWidth, (row + 1) * cellHeight);

                    if (col < colMax && row < rowMax)
                    {
                        bool isBlackCell = cells[col][row].IsBlack;

                        if (isBlackCell && showSolution)
                        {
                            Rectangle rect = new Rectangle()
                            {
                                X = cellWidth * (col + longestRow),
                                Y = cellHeight * (row + longestCol),
                                Width = cellWidth,
                                Height = cellHeight
                            };
                            gr.FillRectangle(Brushes.Black, rect);
                        }
                    }
                    else if (col == colMax || row == rowMax)
                    {
                        if (row < rowMax)
                        {
                            // Row clues
                            List<int> blacks = grid.Rows[row].BlackGroups;
                            int offset = longestRow - blacks.Count;
                            for (int i = 0; i < blacks.Count; i++)
                            {
                                int fontX = (col - colMax + offset + i) * cellWidth;
                                int fontY = (row + longestCol) * cellHeight;

                                string count = blacks[i].ToString();

                                gr.DrawString(count, numberFont, Brushes.Black, fontX, fontY);
                            }
                        }
                        else if (col < colMax)
                        {
                            // Column clues
                            List<int> blacks = grid.Columns[col].BlackGroups;
                            int offset = longestCol - blacks.Count;
                            for (int i = 0; i < blacks.Count; i++)
                            {
                                int fontX = (col + longestRow) * cellWidth;
                                int fontY = (row - rowMax + offset + i) * cellHeight;

                                string count = blacks[i].ToString();

                                gr.DrawString(count, numberFont, Brushes.Black, fontX, fontY);
                            }
                        }
                    }
                }
            }

            return nonogramImage;
        }

        private Font GetFont(int cellWidth, int cellHeight, Graphics g)
        {
            int fontSize = 1;
            SizeF stringSize;
            Font font;
            do
            {
                font = new Font("Arial", fontSize, GraphicsUnit.Pixel);
                stringSize = g.MeasureString("11", font);
                fontSize++;
            } while (stringSize.Width < cellWidth && stringSize.Height < cellHeight);

            return font;
        }
    }
}
