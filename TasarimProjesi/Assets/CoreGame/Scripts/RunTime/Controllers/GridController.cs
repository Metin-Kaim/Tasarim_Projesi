using RunTime.Datas.UnityObjects;
using RunTime.Handlers;
using RunTime.Signals;
using UnityEngine;

namespace RunTime.Controllers
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] Transform gridContainer;
        [SerializeField] EntitySpawnController entitySpawnController;

        private CD_Level _currentLevel;
        private TileHandler[,] _tileHandlersArray;
        private int _row;
        private int _column;

        public Vector2 GetGridSize => new(_row, _column);
        public TileHandler[,] TileHandlersArray => _tileHandlersArray;
        public bool IsGridDone { get; set; }

        private void Awake()
        {
            GridSize gridSize = Resources.Load<CD_Grid>("GridDatas/GridSize").GridSize;
            _row = gridSize.row;
            _column = gridSize.column;

            _tileHandlersArray = new TileHandler[_row, _column];
        }

        private void Start()
        {
            _currentLevel = LevelSignals.Instance.onGetCurrentLevel?.Invoke();

            CreateGrid();
        }

        public void CreateGrid()
        {
            int objID = -1;
            float cellDistance = .75f;

            Vector2 gridSize = GridSignals.Instance.onGetGridSize.Invoke();

            int row = (int)gridSize.x;
            int col = (int)gridSize.y;

            for (int r = 0; r < row; r++)
            {
                GameObject currentRow = new($"Row {r}");
                currentRow.transform.SetParent(gridContainer);
                currentRow.transform.localPosition = new Vector3(0, -(cellDistance * r), 0);

                for (int c = 0; c < col; c++)
                {
                    objID++;
                    if (_currentLevel.LevelEntities.EntitiesList[objID].EntityType == 0) continue;

                    TileHandler currentTile = entitySpawnController.SpawnTile(cellDistance, currentRow, objID, r, c);

                    _tileHandlersArray[r, c] = currentTile;

                    if (!_currentLevel.LevelEntities.EntitiesList[objID].IsStatic)
                    {
                        entitySpawnController.SpawnObject(currentTile);
                    }
                    else
                    {
                        entitySpawnController.SpawnObject(currentTile, _currentLevel.LevelEntities.EntitiesList[objID].EntityType);
                    }
                }
            }
            GridSignals.Instance.onScanGrid?.Invoke();
            InputSignals.Instance.onEnableTouch?.Invoke(.1f);
            IsGridDone = true;

        }
    }
}