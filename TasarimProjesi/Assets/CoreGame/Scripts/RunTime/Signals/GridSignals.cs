using RunTime.Handlers;
using System;
using UnityEngine;

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
    }
}