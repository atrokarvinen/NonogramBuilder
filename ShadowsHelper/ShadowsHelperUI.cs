using ImageProcessing;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShadowsHelper
{
    public partial class ShadowsHelperUI : Form
    {
        private int _ScreenWidth;
        private int _ScreenHeight;
        private Bitmap _OriginalImage;
        private readonly ImageProcessor _ImageProcessor;

        public ShadowsHelperUI()
        {
            InitializeComponent();

            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            ClientSize = new Size(resolution.Width - 100, resolution.Height - 100);

            _ImageProcessor = new ImageProcessor();

            _OriginalImage = (Bitmap)Image.FromFile(@"Samples\TestOriginal.jpg");
            OriginalPB.Image = _OriginalImage;

            Bitmap shadowImage = (Bitmap)Image.FromFile(@"Samples\Test_Shadows.png");
            ResultPB.Image = shadowImage;
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

            // ShadowLevelsLabel
            ShadowLevelsLabel.Location = Prcnt2PxLoc(26, 87);
            ShadowLevelsLabel.Size = new Size(76, 20);
            ShadowLevelsLabel.Text = "Shadow levels";

            // ShadowLevelsInput
            ShadowLevelsInput.Location = Prcnt2PxLoc(31, 87);
            ShadowLevelsInput.Size = new Size(100, 20);
            ShadowLevelsInput.Text = "8";

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

            int shadowLevels;
            if (!int.TryParse(ShadowLevelsInput.Text, out shadowLevels))
            {
                MessageBox.Show($"Cannot convert '{ShadowLevelsInput.Text}' to Int32");
                return;
            }
            if (!(0 < shadowLevels && shadowLevels < 256))
            {
                MessageBox.Show($"Shadow level range is [1, 255]");
                return;
            }

            ProcessingArgs args = new ProcessingArgs()
            {
                Image = _OriginalImage,
                ShadowLevels = shadowLevels,
            };
            Task<Result> processingTask = _ImageProcessor.ProcessAsync(args, progressIndicator);


            Result processingResult = await processingTask;

            PictureBoxSetImage(OriginalPB, processingResult.GrayImage);
            PictureBoxSetImage(ResultPB, processingResult.ResultImage);
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
                openFileDialog.InitialDirectory = "C:\\";
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
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();

                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.

                //ResultPB.Image.Save(@"C:\Users\karviatr\Pictures\test.bmp", ImageFormat.Bmp);
                //ResultPB.Image.Save(@"C:\Users\karviatr\Pictures\test.jpg", ImageFormat.Jpeg);
                //ResultPB.Image.Save(@"C:\Users\karviatr\Pictures\test.png", ImageFormat.Png);

                string fullFileName = saveFileDialog.FileName;
                string imageDirName = Path.GetDirectoryName(fullFileName);
                string imageFileName = Path.GetFileName(fullFileName);
                string imageName = imageFileName.Split('.').First();
                string grayFileName = Path.Combine(imageDirName, imageName + "_Gray.png");
                string resultFileName = Path.Combine(imageDirName, imageName + "_Shadows.png");

                OriginalPB.Image.Save(grayFileName, ImageFormat.Png);
                ResultPB.Image.Save(resultFileName, ImageFormat.Png);

                fs.Close();
            }
        }
    }
}

