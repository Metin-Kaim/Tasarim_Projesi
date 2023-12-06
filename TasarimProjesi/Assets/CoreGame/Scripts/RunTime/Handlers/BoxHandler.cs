using RunTime.Abstracts.Entities;

namespace RunTime.Handlers
{
    public class BoxHandler : AbsEntity, ITouchable
    {
        public void OnMouseDown()
        {
            print("Box");
        }
    }
}