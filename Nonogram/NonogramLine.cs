using System.Collections.Generic;

namespace Nonogram
{
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
}