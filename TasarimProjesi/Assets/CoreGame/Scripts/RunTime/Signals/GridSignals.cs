using RunTime.Handlers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class GridSignals : MonoBehaviour
    {
        #region Singleton
        public static GridSignals Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        #endregion

        public Func<Vector2> onGetGridSize;
        public Func<TileHandler[,]> onGetTileHandlers;
        public UnityAction<TileHandler> onSpawnNewEntity;
        public UnityAction onScanGrid;
    }
}