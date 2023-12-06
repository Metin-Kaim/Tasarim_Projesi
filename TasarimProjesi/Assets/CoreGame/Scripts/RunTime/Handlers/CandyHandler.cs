using RunTime.Abstracts.Entities;

namespace RunTime.Handlers
{
    public class CandyHandler : AbsEntity, ITouchable
    {
        public void OnMouseDown()
        {
            print("Candy");
        }
    }
}