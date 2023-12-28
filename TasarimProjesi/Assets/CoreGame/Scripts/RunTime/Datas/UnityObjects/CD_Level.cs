using RunTime.Datas.ValueObjects;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "new Level", menuName = "Project/New Level")]

    public class CD_Level : ScriptableObject
    {
        public LevelFeatures LevelFeatures;

        public void Reset()
        {
            for (int i = 0; i < LevelFeatures.EntitiesList.Count; i++)
            {
                LevelFeatures.EntitiesList[i].Reset();
            }
        }
    }
}