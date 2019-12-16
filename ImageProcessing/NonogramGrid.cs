using System.Collections.Generic;

namespace ImageProcessing
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

    public class NonogramLine
    {
        public bool IsRow { get; set; }
        public int Location { get; set; }
        public List<int> BlackGroups { get; set; }

        private int prevBlackCoord;
        private int blackGroupCounter;

        public NonogramLine()
        {
            prevBlackCoord = 0;
            blackGroupCounter = 0;
            BlackGroups = new List<int>();
        }

        public void AddCell(int cellCoord, bool isBlack)
        {
            if (isBlack && (prevBlackCoord == 0 || prevBlackCoord == cellCoord - 1))
            {
                blackGroupCounter++;
                prevBlackCoord = cellCoord;
            }
            else if (blackGroupCounter > 0)
            {
                BlackGroups.Add(blackGroupCounter);
                prevBlackCoord = 0;
                blackGroupCounter = 0;
            }
        }
    }

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

    }
}
