using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ImageProcessor
    {
        private Bitmap ConvertPixelFormat(Bitmap image, System.Drawing.Imaging.PixelFormat newFormat)
        {
            Bitmap clone = new Bitmap(image.Width, image.Height, newFormat);

            using (Graphics gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(image, new Rectangle(0, 0, clone.Width, clone.Height));
            }

            return clone;
        }

        public Bitmap ToGrayscale(Bitmap image)
        {
            Grayscale gs = new Grayscale(1.0 / 3, 1.0 / 3, 1.0 / 3);
            return gs.Apply(image);
        }

        public Bitmap Threshold(Bitmap image, int thresholdMin, int thresholdMax)
        {
            Threshold t = new Threshold(thresholdMin);
            Bitmap tMinImage = t.Apply(image);

            t.ThresholdValue = thresholdMax;
            Bitmap tMaxImage = t.Apply(image);

            Subtract s = new Subtract(tMaxImage);
            Bitmap tImage = s.Apply(tMinImage);

            return tImage;
        }

        private void SetPixels(Bitmap image, Bitmap tImage, Color color, Blob blob)
        {
            Rectangle rect = blob.Rectangle;
            for (int i = 0; i < rect.Width; i++)
            {
                int row = rect.X + i;
                for (int j = 0; j < rect.Height; j++)
                {
                    int col = rect.Y + j;
                    Color origC = tImage.GetPixel(row, col);

                    if (origC.B > 0)
                        image.SetPixel(row, col, color);
                }
            }
        }

        private Bitmap ChangeResolution(Bitmap image, int width, int height)
        {
            ResizeNearestNeighbor rnn = new ResizeNearestNeighbor(width, height);
            return rnn.Apply(image);
        }

        public async Task<Result> ProcessAsync(ProcessingArgs args, IProgress<ProgressResult> progress)
        {
            ProgressResult progressResult = new ProgressResult();
            progressResult.ProgressCount = 0;
            Report(progress, progressResult);

            Bitmap image = args.Image;
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                Bitmap convertedImage = ConvertPixelFormat(image, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                image = convertedImage;
            }

            Bitmap grayImage = await Task.Run(() => ToGrayscale(image));

            progressResult.ProgressCount = 10;
            Report(progress, progressResult);

            int tMin = args.Thresholds[0];
            int tMax = args.Thresholds[1];

            Bitmap thresholdImage = Threshold(grayImage, tMin, tMax);

            progressResult.ProgressCount = 20;
            Report(progress, progressResult);

            int rows = 40;
            int cols = 40;
            Size size = thresholdImage.Size;
            int imgWidth = size.Width;
            int imgHeight = size.Height;

            double aspect = 1.0 * imgWidth / imgHeight;
            int newWidth = (int)(cols * aspect);
            int newHeight = rows;

            Bitmap lowResImage = ChangeResolution(thresholdImage, newWidth, newHeight);

            progressResult.ProgressCount = 30;
            Report(progress, progressResult);

            NonogramGrid blackCoordinates = GetBoardCells(lowResImage);

            Bitmap nonogramImage = GetNonogramImage(image.Size, lowResImage.Size, blackCoordinates);

            //ChangeResolution(lowResImage, size.Width, size.Height);
            Bitmap resultImage = nonogramImage;

            Result r = new Result()
            {
                GrayImage = grayImage,
                ResultImage = resultImage,
            };

            progressResult.ProgressCount = 100;
            Report(progress, progressResult);

            return r;
        }

        private NonogramGrid GetBoardCells(Bitmap lowResImage)
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
                }

                grid.Cells.Add(columnCells);
            }

            return grid;
        }

        private Bitmap GetNonogramImage(Size origSize, Size lowResSize, NonogramGrid grid)
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

            Pen pen = new Pen(Brushes.Red);
            Font numberFont = GetFont(cellWidth, cellHeight, gr);

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

                        if (isBlackCell)
                        {
                            Rectangle rect = new Rectangle()
                            {
                                X = cellHeight * (col + longestCol),
                                Y = cellWidth * (row + longestRow),
                                Width = cellWidth,
                                Height = cellHeight
                            };
                            gr.FillRectangle(Brushes.Red, rect);
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
                                int fontX = (col - colMax + offset + i) * cellHeight;
                                int fontY = (row + longestRow) * cellWidth;

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
                                int fontX = (col + longestCol) * cellHeight;
                                int fontY = (row - rowMax + offset + i) * cellWidth;

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

        private void Report(IProgress<ProgressResult> progress, ProgressResult value)
        {
            if (progress != null)
                progress.Report(value);
        }
    }
}
