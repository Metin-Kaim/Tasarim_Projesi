using RunTime.Abstracts.Entities;

namespace RunTime.Handlers
{
    public class BombHandler : AbsEntity, ITouchable
    {
        public void OnMouseDown()
        {
            print("Bomb");
        }
    }
}