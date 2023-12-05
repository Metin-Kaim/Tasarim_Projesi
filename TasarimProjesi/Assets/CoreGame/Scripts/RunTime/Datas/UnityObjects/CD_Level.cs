using RunTime.Datas.ValueObjects;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "new Level", menuName = "Project/New Level")]

    public class CD_Level : ScriptableObject
    {
        public LevelEntities LevelEntities;

        public void Reset()
        {
            for (int i = 0; i < LevelEntities.ObjectsList.Count; i++)
            {
                LevelEntities.ObjectsList[i].Reset();
            }
        }
    }
}