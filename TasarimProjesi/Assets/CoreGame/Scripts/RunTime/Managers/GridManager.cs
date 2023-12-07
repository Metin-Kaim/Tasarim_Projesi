using DG.Tweening;
using RunTime.Abstracts.Entities;
using RunTime.Datas.UnityObjects;
using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

namespace RunTime.Managers
{
    public class GridManager : MonoBehaviour
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        [SerializeField] private Transform gridContainer;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject[] entitiesArray;

        private TileHandler[,] _tileHandlersArray;
        private int _totalObjectCount;
        private CD_Level _currentLevel;
        bool _isSpawningDone;

        private void Awake()
        {
            GridSize gridSize = Resources.Load<CD_Grid>("GridDatas/GridSize").GridSize;
            Row = gridSize.row;
            Column = gridSize.column;

            _tileHandlersArray = new TileHandler[Row, Column];

            _totalObjectCount = entitiesArray.Count(x => x.CompareTag("Candy"));
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            GridSignals.Instance.onGetTileHandlers += () => _tileHandlersArray;
            GridSignals.Instance.onGetGridSize += () => new Vector2(Row, Column);
            GridSignals.Instance.onSpawnNewEntity += SpawnObject;
        }

        private void UnSibscribeEvents()
        {
            GridSignals.Instance.onGetTileHandlers -= () => _tileHandlersArray;
            GridSignals.Instance.onGetGridSize -= () => new Vector2(Row, Column);
            GridSignals.Instance.onSpawnNewEntity -= SpawnObject;
        }
        private void OnDisable()
        {
            UnSibscribeEvents();
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

            for (int r = 0; r < Row; r++)
            {
                GameObject currentRow = new($"Row {r}");
                currentRow.transform.SetParent(gridContainer);
                currentRow.transform.localPosition = new Vector3(0, -(cellDistance * r), 0);

                for (int c = 0; c < Column; c++)
                {
                    objID++;
                    if (_currentLevel.LevelEntities.EntitiesList[objID].EntityType == 0) continue;

                    TileHandler currentTile = SpawnTile(cellDistance, currentRow, objID, r, c);

                    _tileHandlersArray[r, c] = currentTile;

                    if (!_currentLevel.LevelEntities.EntitiesList[objID].IsStatic)
                    {
                        SpawnObject(currentTile);
                    }
                    else
                    {
                        SpawnObject(currentTile, _currentLevel.LevelEntities.EntitiesList[objID].EntityType);
                    }
                }
            }
            _isSpawningDone = true;
        }

        private void SpawnObject(TileHandler currentTile)
        {
            GameObject newObject = Instantiate(entitiesArray[Random.Range(0, _totalObjectCount)], currentTile.transform);
            SetObjectFeatures(currentTile, newObject);
        }
        private void SpawnObject(TileHandler currentTile, EntitiesEnum objectType) // Her objeye özel script yaz
        {
            GameObject newObject = Instantiate(entitiesArray.FirstOrDefault(x => x.GetComponent<AbsEntity>().EntityType == objectType), currentTile.transform);
            SetObjectFeatures(currentTile, newObject);
        }
        private void SetObjectFeatures(TileHandler currentTile, GameObject newObject)
        {
            AbsEntity absEntity = newObject.GetComponent<AbsEntity>();
            ITouchable touchable = newObject.GetComponent<ITouchable>();
            absEntity.SetFeatures(currentTile.Id, currentTile.Row, currentTile.Column);
            touchable.CurrentTile = currentTile;
            currentTile.CurrentEntity = absEntity;

            if (_isSpawningDone)
                newObject.transform.DOLocalMoveY(0, .5f).From(50);
        }
        private TileHandler SpawnTile(float cellDistance, GameObject currentRow, int id, int row, int col)
        {
            TileHandler tileHandler = Instantiate(tilePrefab, currentRow.transform).GetComponent<TileHandler>();
            tileHandler.transform.localPosition = new(cellDistance * col, 0, 0);
            tileHandler.name = $"Tile {row}-{col}";
            tileHandler.GetComponent<AbsEntity>().SetFeatures(id, row, col);


            return tileHandler;
        }
    }
}