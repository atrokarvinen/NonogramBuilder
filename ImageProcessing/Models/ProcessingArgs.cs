using System.Drawing;

namespace ImageProcessing
{
    public class ProcessingArgs
    {
        public Bitmap Image { get; set; }
        public int[] Thresholds { get; set; }
        public int ColumnCount { get; set; }
    }
}