using RunTime.Datas.UnityObjects;
using RunTime.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Managers
{
    public class GridManager : MonoBehaviour
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Transform GridContainer;
        public GameObject TilePrefab;
        public List<GameObject> TileList;
        private CD_Level _currentLevel;

        private void Awake()
        {
            GridSize gridSize = Resources.Load<CD_Grid>("GridDatas/GridSize").GridSize;
            Row = gridSize.row;
            Column = gridSize.column;
        }

        private void Start()
        {
            _currentLevel = LevelSignals.Instance.onGetCurrentLevel?.Invoke();
            print(_currentLevel);

            CreateGrid();

        }

        public void CreateGrid()
        {
            int counter = -1;
            float cellDistance = .75f;

            //GridContainer.transform.position = new(0, 0, 0);

            for (int i = 0; i < Row; i++)
            {
                GameObject currentRow = new($"Row {i}");
                //currentRow.transform.position = Vector3.zero;
                currentRow.transform.SetParent(GridContainer);
                currentRow.transform.localPosition = new Vector3(0, -(cellDistance * i), 0);

                for (int j = 0; j < Column; j++)
                {
                    counter++;

                    if (_currentLevel.LevelEntities.ObjectsList[counter].ObjectType == 0) continue;

                    GameObject currentTile = Instantiate(TilePrefab, currentRow.transform);
                    currentTile.transform.localPosition = new(cellDistance * j, 0, 0);
                    currentTile.name = $"Tile {i}-{j}";
                }
            }
        }
    }
}