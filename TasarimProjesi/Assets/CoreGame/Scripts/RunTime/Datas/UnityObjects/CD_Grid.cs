using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName ="new Grid", menuName ="Project/New Grid")]
    public class CD_Grid : ScriptableObject
    {
        public GridSize GridSize;
    }
}