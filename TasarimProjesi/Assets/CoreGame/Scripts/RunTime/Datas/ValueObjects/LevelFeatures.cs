using System.Collections.Generic;


namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public class LevelFeatures
    {
        public List<EntityFeatures> EntitiesList;
        public AllowedCandies AllowedCandies;
        public List<LevelGoals> LevelGoals;
        public LevelMoveCount LevelMoveCount;
    }
}