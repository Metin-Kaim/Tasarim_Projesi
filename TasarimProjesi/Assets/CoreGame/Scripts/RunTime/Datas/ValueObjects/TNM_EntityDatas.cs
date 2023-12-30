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
    public struct TNM_EntityDatas 
    {
        public EntitiesEnum EntityType;
        public Texture TextureData;
        public Sprite SpriteData;
        public GameObject EntityData;
    }
}