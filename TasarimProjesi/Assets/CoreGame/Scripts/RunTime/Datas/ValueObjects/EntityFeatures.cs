
using RunTime.Enums;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public class EntityFeatures
    {
        public ObjectsEnum ObjectType;
        public bool IsStatic;

        public void SetFeatures(ObjectsEnum ObjectType, bool IsStatic)
        {
            this.ObjectType = ObjectType;
            this.IsStatic = IsStatic;
        }
        public void Reset()
        {
            ObjectType = ObjectsEnum.None;
            IsStatic = false;
        }
    }
}