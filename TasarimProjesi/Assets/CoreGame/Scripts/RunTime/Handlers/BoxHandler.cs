using RunTime.Abstracts.Entities;
using UnityEngine;

namespace RunTime.Handlers
{
    public class BoxHandler : AbsEntity, ITouchable
    {
        [SerializeField] private TileHandler _currentTile;

        public TileHandler CurrentTile { get => _currentTile; set => _currentTile = value; }

        public void OnMouseDown()
        {
            print("Box");
        }
    }
}