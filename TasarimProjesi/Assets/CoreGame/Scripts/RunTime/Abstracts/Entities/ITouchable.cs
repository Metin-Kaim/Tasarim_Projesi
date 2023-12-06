using RunTime.Handlers;

namespace RunTime.Abstracts.Entities
{
    internal interface ITouchable
    {
        public TileHandler CurrentTile { get; set; }
        void OnMouseDown();
    }
}
