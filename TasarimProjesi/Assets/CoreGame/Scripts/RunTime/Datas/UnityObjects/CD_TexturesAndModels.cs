using RunTime.Datas.ValueObjects;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName = "new SpritesAndModels", menuName = "Project/New SpritesAndModels")]
    public class CD_TexturesAndModels : ScriptableObject
    {
        public TNM_EntityDatas[] ObjectDatas;
    }
}