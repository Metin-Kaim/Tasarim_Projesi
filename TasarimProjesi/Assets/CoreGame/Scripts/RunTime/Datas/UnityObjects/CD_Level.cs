using RunTime.Datas.ValueObjects;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "new Level", menuName = "Project/New Level")]

    public class CD_Level : ScriptableObject
    {
        public LevelEntities LevelEntities;
    }
}