using System.Collections.Generic;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public struct AllowedCandies
    {
        public bool Candy1;
        public bool Candy2;
        public bool Candy3;
        public bool Candy4;

        public void SetAllows(bool candy1, bool candy2, bool candy3, bool candy4)
        {
            Candy1 = candy1;
            Candy2 = candy2;
            Candy3 = candy3;
            Candy4 = candy4;
        }
        public void ClearAllows()
        {
            Candy1 = false;
            Candy2 = false;
            Candy3 = false;
            Candy4 = false;
        }
    }
}
