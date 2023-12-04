using RunTime.Enums;
using UnityEngine;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public struct ObjectDatas 
    {
        public ObjectsEnum ObjectType;
        public Texture TextureData;
        public GameObject ObjectData;
    }
}