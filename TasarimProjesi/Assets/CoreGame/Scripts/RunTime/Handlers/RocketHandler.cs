using RunTime.Abstracts.Entities;

namespace RunTime.Handlers
{
    public class RocketHandler : AbsEntity, ITouchable
    {
        public void OnMouseDown()
        {
            print("Rocket");
        }
    }
}