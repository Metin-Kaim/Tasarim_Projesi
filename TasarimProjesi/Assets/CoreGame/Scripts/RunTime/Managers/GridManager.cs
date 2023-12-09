using RunTime.Controllers;
using RunTime.Signals;
using UnityEngine;

namespace RunTime.Managers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] EntitySpawnController spawnController;
        [SerializeField] GridController gridController;
        [SerializeField] ScanController scanController;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            GridSignals.Instance.onGetTileHandlers += () => gridController.TileHandlersArray;
            GridSignals.Instance.onGetGridSize += () => gridController.GetGridSize;
            GridSignals.Instance.onSpawnNewEntity += spawnController.SpawnObject;
            GridSignals.Instance.onScanGrid += scanController.ScanForCombo;
        }

        private void UnSibscribeEvents()
        {
            GridSignals.Instance.onGetTileHandlers -= () => gridController.TileHandlersArray;
            GridSignals.Instance.onGetGridSize -= () => gridController.GetGridSize;
            GridSignals.Instance.onSpawnNewEntity -= spawnController.SpawnObject;
            GridSignals.Instance.onScanGrid -= scanController.ScanForCombo;
        }
        private void OnDisable()
        {
            UnSibscribeEvents();
        }
    }
}