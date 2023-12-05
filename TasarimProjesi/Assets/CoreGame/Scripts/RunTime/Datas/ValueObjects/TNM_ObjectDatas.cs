using RunTime.Enums;
/// <summary>
/// ------------------------
/// TNM: Textures And Models
/// 
/// ------------------------
/// </summary>

using UnityEngine;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public struct TNM_ObjectDatas 
    {
        public ObjectsEnum ObjectType;
        public Texture TextureData;
        public GameObject ObjectData;
    }
}