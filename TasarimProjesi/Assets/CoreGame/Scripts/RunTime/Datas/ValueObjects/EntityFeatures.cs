
using RunTime.Enums;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public class EntityFeatures
    {
        public EntitiesEnum EntityType;
        public bool IsStatic;

        public void SetFeatures(EntitiesEnum EntityType, bool IsStatic)
        {
            this.EntityType = EntityType;
            this.IsStatic = IsStatic;
        }
        public void Reset()
        {
            EntityType = EntitiesEnum.None;
            IsStatic = false;
        }
    }
}