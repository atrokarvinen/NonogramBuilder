using AForge.Imaging.Filters;
using System;
using System.Drawing;
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
            int cols = args.ColumnCount;
            //cols = 40;
            Size size = thresholdImage.Size;
            int imgWidth = size.Width;
            int imgHeight = size.Height;

            double aspect = 1.0 * imgWidth / imgHeight;
            //int newWidth = (int)(cols * aspect);
            //int newHeight = rows;
            int newWidth = cols;
            int newHeight = (int)(cols / aspect);

            Bitmap resultImage = ChangeResolution(thresholdImage, newWidth, newHeight);

            progressResult.ProgressCount = 30;
            Report(progress, progressResult);

            Result r = new Result()
            {
                GrayImage = grayImage,
                ResultImage = resultImage,
            };

            progressResult.ProgressCount = 100;
            Report(progress, progressResult);

            return r;
        }

        private void Report(IProgress<ProgressResult> progress, ProgressResult value)
        {
            if (progress != null)
                progress.Report(value);
        }
    }
}
