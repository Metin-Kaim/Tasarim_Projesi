using RunTime.Datas.UnityObjects;
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

        private void Awake()
        {
            GridSize gridSize = Resources.Load<CD_Grid>("GridDatas/GridSize").GridSize;
            Row = gridSize.row;
            Column = gridSize.column;
            CreateGrid();
        }

        public void CreateGrid()
        {
            for (int i = 0; i < Row; i++)
            {
                GameObject currentRow = new($"Row {i}");
                currentRow.transform.SetParent(GridContainer);
                currentRow.transform.localPosition = new Vector3(currentRow.transform.localPosition.x, currentRow.transform.localPosition.y + (.7f * i), currentRow.transform.localPosition.z);

                for (int j = 0; j < Column; j++)
                {
                    GameObject currentTile = Instantiate(TilePrefab, currentRow.transform);
                    currentTile.transform.localPosition = new(currentTile.transform.localPosition.x + (.7f * j), currentTile.transform.localPosition.y, currentTile.transform.localPosition.z);
                    currentTile.name = $"Tile {i}-{j}";
                }
            }
        }
    }
}