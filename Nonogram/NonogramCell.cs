namespace Nonogram
{
    public class NonogramCell
    {
        public NonogramCell(int row, int col, bool isBlack)
        {
            Row = row;
            Column = col;
            IsBlack = isBlack;
        }

        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsBlack { get; set; }
    }
}