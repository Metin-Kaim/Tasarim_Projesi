using RunTime.Controllers;
using RunTime.Signals;
using UnityEngine;

namespace RunTime.Managers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] EntitySpawnController spawnController;
        [SerializeField] GridController gridController;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            GridSignals.Instance.onGetTileHandlers += () => gridController.TileHandlersArray;
            GridSignals.Instance.onGetGridSize += () => gridController.GetGridSize;
            GridSignals.Instance.onSpawnNewEntity += spawnController.SpawnObject;
        }

        private void UnSibscribeEvents()
        {
            GridSignals.Instance.onGetTileHandlers -= () => gridController.TileHandlersArray;
            GridSignals.Instance.onGetGridSize -= () => gridController.GetGridSize;
            GridSignals.Instance.onSpawnNewEntity -= spawnController.SpawnObject;
        }
        private void OnDisable()
        {
            UnSibscribeEvents();
        }
    }
}