using RunTime.Abstracts.Entities;
using RunTime.Datas.UnityObjects;
using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private List<GameObject> tileList;
        [SerializeField] private GameObject[] objectsArray;

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
                    if (_currentLevel.LevelEntities.ObjectsList[objID].ObjectType == 0) continue;

                    GameObject currentTile = SpawnTile(cellDistance, currentRow, objID, r, c);

                    if (!_currentLevel.LevelEntities.ObjectsList[objID].IsStatic)
                    {
                        SpawnObject(currentTile, objID, r, c);
                    }
                    else
                    {
                        SpawnObject(currentTile, _currentLevel.LevelEntities.ObjectsList[objID].ObjectType, objID, r, c);
                    }


                }
            }
        }

        private void SpawnObject(GameObject currentTile, int id, int row, int col)
        {
            GameObject newObject = Instantiate(objectsArray[Random.Range(0, objectsArray.Length)], currentTile.transform);
            newObject.GetComponent<AbsEntity>().SetFeatures(id, row, col);
        }
        private void SpawnObject(GameObject currentTile, ObjectsEnum objectType, int id, int row, int col) // Her objeye özel script yaz
        {
            GameObject newObject = Instantiate(objectsArray.FirstOrDefault(x => x.GetComponent<AbsEntity>().ObjectType == objectType), currentTile.transform);
        }
        private GameObject SpawnTile(float cellDistance, GameObject currentRow, int id, int row, int col)
        {
            GameObject currentTile = Instantiate(tilePrefab, currentRow.transform);
            currentTile.transform.localPosition = new(cellDistance * col, 0, 0);
            currentTile.name = $"Tile {row}-{col}";
            currentTile.GetComponent<AbsEntity>().SetFeatures(id, row, col);

            tileList.Add(currentTile);

            return currentTile;
        }
    }
}