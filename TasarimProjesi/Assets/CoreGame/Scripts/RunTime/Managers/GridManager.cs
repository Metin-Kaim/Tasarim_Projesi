using DG.Tweening;
using RunTime.Abstracts.Entities;
using RunTime.Controllers;
using RunTime.Datas.UnityObjects;
using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using System.Linq;
using UnityEngine;

namespace RunTime.Managers
{
    public class GridManager : MonoBehaviour
    {

        [SerializeField] EntitySpawnController spawnController;

        private TileHandler[,] _tileHandlersArray;

        public int Row { get; private set; }
        public int Column { get; private set; }

        private void Awake()
        {
            GridSize gridSize = Resources.Load<CD_Grid>("GridDatas/GridSize").GridSize;
            Row = gridSize.row;
            Column = gridSize.column;

            _tileHandlersArray = new TileHandler[Row, Column];

        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            GridSignals.Instance.onGetTileHandlers += () => _tileHandlersArray;
            GridSignals.Instance.onGetGridSize += () => new Vector2(Row, Column);
            GridSignals.Instance.onSpawnNewEntity += spawnController.SpawnObject;
        }

        private void UnSibscribeEvents()
        {
            GridSignals.Instance.onGetTileHandlers -= () => _tileHandlersArray;
            GridSignals.Instance.onGetGridSize -= () => new Vector2(Row, Column);
            GridSignals.Instance.onSpawnNewEntity -= spawnController.SpawnObject;
        }
        private void OnDisable()
        {
            UnSibscribeEvents();
        }
    }
}