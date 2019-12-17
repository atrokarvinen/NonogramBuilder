using ImageProcessing;
using Nonogram;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NonogramBuilder
{
    public partial class NonogramUI : Form
    {
        private int _ScreenWidth;
        private int _ScreenHeight;
        private Bitmap _OriginalImage;
        private Bitmap _NonogramClueImage;
        private readonly ImageProcessor _ImageProcessor;

        public NonogramUI()
        {
            InitializeComponent();

            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            ClientSize = new Size(resolution.Width - 100, resolution.Height - 100);

            _ImageProcessor = new ImageProcessor();

            _OriginalImage = (Bitmap)Image.FromFile(@"Samples\Dragon2.jpg");
            OriginalPB.Image = _OriginalImage;

            Bitmap solutionImage = (Bitmap)Image.FromFile(@"Samples\Dragon2_Solution.png");
            ResultPB.Image = solutionImage;
        }

        private void InitializeMainScreen()
        {
            _ScreenWidth = ClientSize.Width;
            _ScreenHeight = ClientSize.Height;

            // OriginalPB
            OriginalPB.Location = Prcnt2PxLoc(2, 3);
            OriginalPB.Size = Prcnt2PxSize(47, 80);
            OriginalPB.BorderStyle = BorderStyle.FixedSingle;
            OriginalPB.SizeMode = PictureBoxSizeMode.StretchImage;

            // ResultPB
            ResultPB.Location = Prcnt2PxLoc(51, 3);
            ResultPB.Size = Prcnt2PxSize(47, 80);
            ResultPB.BorderStyle = BorderStyle.FixedSingle;
            ResultPB.SizeMode = PictureBoxSizeMode.StretchImage;

            // OriginalImageLabel
            OriginalImageLabel.Location = Prcnt2PxLoc(2, 1);
            OriginalImageLabel.Size = new Size(97, 13);
            OriginalImageLabel.Text = "Original image";

            // ResultImageLabel
            ResultImageLabel.Location = Prcnt2PxLoc(51, 1);
            ResultImageLabel.Size = new Size(92, 13);
            ResultImageLabel.Text = "Shadow image";

            // LoadImageButton
            LoadImageButton.Location = Prcnt2PxLoc(2, 85);
            LoadImageButton.Size = Prcnt2PxSize(5, 5);
            LoadImageButton.Text = "Load image";

            // ComputeButton
            ComputeButton.Location = Prcnt2PxLoc(10, 85);
            ComputeButton.Size = Prcnt2PxSize(5, 5);
            ComputeButton.Text = "Compute";

            // SaveImageButton
            SaveImageButton.Location = Prcnt2PxLoc(18, 85);
            SaveImageButton.Size = Prcnt2PxSize(5, 5);
            SaveImageButton.Text = "Save image";

            // ThresholdMinLabel
            ThresholdMinLabel.Location = Prcnt2PxLoc(26, 87);
            ThresholdMinLabel.Size = new Size(76, 20);

            // ThresholdMinInput
            ThresholdMinInput.Location = Prcnt2PxLoc(32, 87);
            ThresholdMinInput.Size = new Size(100, 20);
            ThresholdMinInput.Text = "100";

            // ThresholdMaxLabel
            ThresholdMaxLabel.Location = Prcnt2PxLoc(40, 87);
            ThresholdMaxLabel.Size = new Size(76, 20);

            // ThresholdMaxInput
            ThresholdMaxInput.Location = Prcnt2PxLoc(46, 87);
            ThresholdMaxInput.Size = new Size(100, 20);
            ThresholdMaxInput.Text = "255";

            // ColumnCountLabel
            ColumnCountLabel.Location = Prcnt2PxLoc(54, 87);
            ColumnCountLabel.Size = new Size(47, 13);
            ColumnCountLabel.Text = "Columns";
            
            // ColumnCountInput
            ColumnCountInput.Location = Prcnt2PxLoc(60, 87);
            ColumnCountInput.Size = new Size(100, 20);
            ColumnCountInput.Text = "30";

            // ProgressBarLabel
            ProgressBarLabel.Location = Prcnt2PxLoc(48, 90);
            ProgressBarLabel.Size = new Size(90, 13);
            ProgressBarLabel.Text = "Processing progress";

            // ProgressBar
            ProgressBar.Location = Prcnt2PxLoc(2, 92);
            ProgressBar.Size = Prcnt2PxSize(96, 2);
        }

        private Point Prcnt2PxLoc(double percentWidth, double percentHeight)
        {
            int width = (int)Math.Round(_ScreenWidth * percentWidth / 100.0);
            int height = (int)Math.Round(_ScreenHeight * percentHeight / 100.0);
            return new Point(width, height);
        }

        private Size Prcnt2PxSize(double percentWidth, double percentHeight)
        {
            Point p = Prcnt2PxLoc(percentWidth, percentHeight);
            return new Size(p.X, p.Y);
        }

        private void ShadowsHelperUI_Resize(object sender, EventArgs e)
        {
            InitializeMainScreen();
        }

        private async void ComputeButton_Click(object sender, EventArgs e)
        {
            Progress<ProgressResult> progressIndicator = new Progress<ProgressResult>(ReportProgress);

            int[] thresholds = new int[2];
            int columnCount;

            if (!(TryParseString(ThresholdMinInput.Text, out thresholds[0]) &&
                TryParseString(ThresholdMaxInput.Text, out thresholds[1]) &&
                TryParseString(ColumnCountInput.Text, out columnCount)))
            {
                return;
            }

            ProcessingArgs args = new ProcessingArgs()
            {
                Image = _OriginalImage,
                Thresholds = thresholds,
                ColumnCount = columnCount
            };
            Task<Result> processingTask = _ImageProcessor.ProcessAsync(args, progressIndicator);

            Result processingResult = await processingTask;

            Bitmap lowResImage = processingResult.ResultImage;
            NonogramGrid ng = new NonogramGrid();
            NonogramGrid blackCoordinates = ng.GetBoardCells(lowResImage);

            _NonogramClueImage?.Dispose();
            _NonogramClueImage = ng.NonogramFromImage(_OriginalImage.Size, lowResImage.Size, blackCoordinates, false);
            Bitmap nonogramSolvedImage = ng.NonogramFromImage(_OriginalImage.Size, lowResImage.Size, blackCoordinates, true);

            PictureBoxSetImage(OriginalPB, processingResult.GrayImage);
            PictureBoxSetImage(ResultPB, nonogramSolvedImage);
        }

        void ReportProgress(ProgressResult value)
        {
            ProgressBar.Value = value.ProgressCount;
            ProgressBarLabel.Text = $"Processing progress: {value.ProgressCount}%";
        }

        private void PictureBoxSetImage(PictureBox pb, Bitmap image)
        {
            if (pb.Image != null && pb.Image != _OriginalImage)
                pb.Image.Dispose();

            pb.Image = image;
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.tiff, *.png, *.bmp) | *.jpg; *.jpeg; *.tiff; *.png; *.bmp";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;

                    _OriginalImage = (Bitmap)Image.FromFile(filePath);
                    PictureBoxSetImage(OriginalPB, _OriginalImage);
                }
            }
        }

        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Png Image|*.png";
            saveFileDialog.Title = "Save an Image File";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                FileStream fs = (FileStream)saveFileDialog.OpenFile();

                string fullFileName = saveFileDialog.FileName;
                string imageDirName = Path.GetDirectoryName(fullFileName);
                string imageFileName = Path.GetFileName(fullFileName);
                string imageName = imageFileName.Split('.').First();
                string clueFile = Path.Combine(imageDirName, imageName + "_NonogramClues.png");
                string solutionName = Path.Combine(imageDirName, imageName + "_Solution.png");

                _NonogramClueImage?.Save(clueFile, ImageFormat.Png);
                ResultPB.Image.Save(solutionName, ImageFormat.Png);

                fs.Close();
            }
        }

        private bool TryParseString<T>(string text, out T retValue)
        {
            try
            {
                retValue = (T)Convert.ChangeType(text, typeof(T));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot convert '{text}' to type {typeof(T)}");
            }
            retValue = default;
            return false;
        }
    }
}

