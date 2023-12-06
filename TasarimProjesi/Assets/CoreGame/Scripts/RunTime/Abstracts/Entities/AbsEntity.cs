using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Abstracts.Entities
{
    public abstract class AbsEntity : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private int _row;
        [SerializeField] private int _column;
        [SerializeField] private EntitiesEnum _entityType;

        protected Vector2 gridSize;
        protected TileHandler[,] tileHandlersArray;

        public int Id { get => _id; set => _id = value; }
        public int Row { get => _row; set => _row = value; }
        public int Column { get => _column; set => _column = value; }
        public EntitiesEnum EntityType => _entityType;

        public static List<List<int>> _chosenCandies = new();

        protected virtual void Start()
        {
            gridSize = GridSignals.Instance.onGetGridSize.Invoke();
            tileHandlersArray = GridSignals.Instance.onGetTileHandlers.Invoke();
        }

        public void SetFeatures(int id, int row, int column)
        {
            Id = id;
            Row = row;
            Column = column;
        }

    }
}