
using RunTime.Enums;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public class EntityFeatures
    {
        public EntitiesEnum EntityType;
        public bool IsStatic;

        public void SetFeatures(EntitiesEnum EntityTpye, bool IsStatic)
        {
            this.EntityType = EntityTpye;
            this.IsStatic = IsStatic;
        }
        public void Reset()
        {
            EntityType = EntitiesEnum.None;
            IsStatic = false;
        }
    }
}