using DG.Tweening;
using RunTime.Abstracts.Entities;
using RunTime.Enums;
using RunTime.Handlers;
using System.Linq;
using UnityEngine;

namespace RunTime.Controllers
{
    public class EntitySpawnController : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject[] entitiesArray;

        private int _totalObjectCount;

        private void Awake()
        {
            _totalObjectCount = entitiesArray.Count(x => x.CompareTag("Candy"));
        }

        public void SpawnObject(TileHandler currentTile)
        {
            GameObject newObject = Instantiate(entitiesArray[Random.Range(0, _totalObjectCount)], currentTile.transform);
            SetObjectFeatures(currentTile, newObject);
        }
        public void SpawnObject(TileHandler currentTile, EntitiesEnum objectType) // Her objeye özel script yaz
        {
            GameObject newObject = Instantiate(entitiesArray.FirstOrDefault(x => x.GetComponent<AbsEntity>().EntityType == objectType), currentTile.transform);
            SetObjectFeatures(currentTile, newObject);
        }
        private void SetObjectFeatures(TileHandler currentTile, GameObject newObject)
        {
            AbsEntity absEntity = newObject.GetComponent<AbsEntity>();
            absEntity.SetFeatures(currentTile.Id, currentTile.Row, currentTile.Column);
            absEntity.CurrentTile = currentTile;
            currentTile.CurrentEntity = absEntity;

            if (absEntity.MoveTweener != null && absEntity.MoveTweener.IsPlaying())
            {
                absEntity.MoveTweener.Complete();
            }
            absEntity.MoveTweener = absEntity.transform.DOLocalMoveY(0, .4f).From(50).OnComplete(() => absEntity.MoveTweener = null);
        }
        public TileHandler SpawnTile(float cellDistance, GameObject currentRow, int id, int row, int col)
        {
            TileHandler tileHandler = Instantiate(tilePrefab, currentRow.transform).GetComponent<TileHandler>();
            tileHandler.transform.localPosition = new(cellDistance * col, 0, 0);
            tileHandler.name = $"Tile {row}-{col}";
            tileHandler.SetFeatures(id, row, col);


            return tileHandler;
        }
    }
}