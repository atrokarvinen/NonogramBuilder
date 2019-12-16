using System.Drawing;

namespace ImageProcessing
{
    public class ProgressResult
    {
        public int ProgressCount { get; set; }
        public string CurrentPhase { get; set; }
        public Bitmap CurrentImage { get; set; }
    }
}