using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

//TODO

/*

    Buttons:
    Load image dialog
    Save
    Colour to grayscale

    Two pictureboxes:
    Left image: Original image
    - Label: Original image
    Right image: Result image
    - Label: Shadow image

    Input: amount of shadow levels

    Load a default original image and result image

    Error logging in UI. Inside multiline textbox. Label: process information.
    Processingbar in UI. Label: processing progress %. Visible only when processing.

 */

namespace ImageProcessing
{
    public class ImageProcessor
    {
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

            //tMinImage.Save(@"C:\Users\karviatr\Pictures\tMin.bmp", ImageFormat.Bmp);
            //tMaxImage.Save(@"C:\Users\karviatr\Pictures\tMax.bmp", ImageFormat.Bmp);
            //tImage.Save(@"C:\Users\karviatr\Pictures\tImage.bmp", ImageFormat.Bmp);

            return tImage;
        }

        private Blob[] GetBlobs(Bitmap image)
        {
            BlobCounterBase bc = new BlobCounter()
            {
                FilterBlobs = true,
                MinWidth = 1,
                MinHeight= 1,
            };
            bc.ProcessImage(image);
            return bc.GetObjectsInformation();
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

        public async Task<Result> ProcessAsync(ProcessingArgs args, IProgress<ProgressResult> progress)
        {
            ProgressResult progressResult = new ProgressResult();
            progressResult.ProgressCount = 0;
            Report(progress, progressResult);

            Bitmap image = args.Image;
            Bitmap grayImage = await Task.Run(() => ToGrayscale(image));

            Report(progress, progressResult);

            int progressCount = 10;
            int phaseProgressMax = 80;
            int progressStepIncrement = (int)Math.Floor(1.0 / args.ShadowLevels * phaseProgressMax);
            Bitmap result = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            for (int i = 0; i < args.ShadowLevels; i++)
            {
                int thresholdMin = i * 255 / args.ShadowLevels;
                int thresholdMax = Math.Min((i + 1) * 255 / args.ShadowLevels, 255);
                Bitmap tImage = await Task.Run(() => Threshold(grayImage, thresholdMin, thresholdMax));

                int grayVal = 255 - thresholdMin;
                Color c = Color.FromArgb(grayVal, 0, 0, 0);

                Blob[] blobs = GetBlobs(tImage);
                for (int j = 0; j < blobs.Length; j++)
                {
                    SetPixels(result, tImage, c, blobs[j]);
                }

                progressCount += progressStepIncrement;
                progressResult.ProgressCount = progressCount;
                Report(progress, progressResult);
            }

            Bitmap resultImage = result;
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
