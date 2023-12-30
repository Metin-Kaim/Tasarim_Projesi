//TODO:: FIX IT
using DG.Tweening;
using RunTime.Abstracts.Entities;
using RunTime.Datas.UnityObjects;
using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RunTime.Controllers
{
    public class EntitySpawnController : MonoBehaviour
    {
        [SerializeField] GridController gridController;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject[] entitiesArray;

        //private int _totalObjectCount;
        private List<GameObject> _spawnableObjects;
        private CD_Level _level;
        private CD_TexturesAndModels _cd_TexturesAndModels;
        private void Awake()
        {
            //_totalObjectCount = entitiesArray.Count(x => x.CompareTag("Candy"));
            _spawnableObjects = new();
            _cd_TexturesAndModels = Resources.Load<CD_TexturesAndModels>("TexturesAndModels/TexturesAndModels");
        }

        private void Start() //Fix it
        {
            _level = LevelSignals.Instance.onGetCurrentLevel();

            if (_level.LevelFeatures.AllowedCandies.Candy1) // make it a list
            {
                _spawnableObjects.Add(entitiesArray[0]);
            }
            if (_level.LevelFeatures.AllowedCandies.Candy2)
            {
                _spawnableObjects.Add(entitiesArray[1]);
            }
            if (_level.LevelFeatures.AllowedCandies.Candy3)
            {
                _spawnableObjects.Add(entitiesArray[2]);
            }
            if (_level.LevelFeatures.AllowedCandies.Candy4)
            {
                _spawnableObjects.Add(entitiesArray[3]);
            }

            int goalCount = _level.LevelFeatures.LevelGoals.Count;
            print(goalCount);
            for (int i = 0; i < goalCount; i++)
            {
                Datas.ValueObjects.LevelGoals levelGoals = _level.LevelFeatures.LevelGoals[i];
                Datas.ValueObjects.TNM_EntityDatas tNM_EntityDatas = _cd_TexturesAndModels.ObjectDatas.FirstOrDefault(x => x.EntityType == levelGoals.entityType);
                UISignals.Instance.onSpawnNewGoal?.Invoke
                    (tNM_EntityDatas.SpriteData, tNM_EntityDatas.EntityType,
                     levelGoals.entityCount);
            }
        }

        public void SpawnObject(TileHandler currentTile)
        {
            GameObject newObject = Instantiate(_spawnableObjects[Random.Range(0, _spawnableObjects.Count)], currentTile.transform);
            SetObjectFeatures(currentTile, newObject, true);
        }
        public void SpawnObject(TileHandler currentTile, EntitiesEnum objectType)
        {
            GameObject newObject = Instantiate(_cd_TexturesAndModels.ObjectDatas.FirstOrDefault(x => x.EntityType == objectType).EntityData, currentTile.transform);
            SetObjectFeatures(currentTile, newObject, false);
        }

        private void SetObjectFeatures(TileHandler currentTile, GameObject newObject, bool isAnim)
        {
            AbsEntity absEntity = newObject.GetComponent<AbsEntity>();
            absEntity.SetFeatures(currentTile.Id, currentTile.Row, currentTile.Column);
            absEntity.CurrentTile = currentTile;
            currentTile.CurrentEntity = absEntity;

            if (!gridController.IsGridDone || !isAnim) return;

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