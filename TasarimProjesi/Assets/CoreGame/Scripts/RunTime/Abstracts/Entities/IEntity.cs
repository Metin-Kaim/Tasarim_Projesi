
namespace RunTime.Abstracts.Entities
{
    public interface IEntity
    {
        int Id { get; set; }
        int Row { get; set; }
        int Column { get; set; }

        void SetFeatures(int id, int row, int col);
    }
}
