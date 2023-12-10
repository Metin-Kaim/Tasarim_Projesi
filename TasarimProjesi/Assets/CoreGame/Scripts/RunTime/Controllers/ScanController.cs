using JetBrains.Annotations;
using RunTime.Abstracts.Entities;
using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Controllers
{
    public class ScanController : MonoBehaviour
    {
        Vector2 _gridSize;
        TileHandler[,] _tileHandlers;

        private void Start()
        {
            _gridSize = GridSignals.Instance.onGetGridSize.Invoke();
            _tileHandlers = GridSignals.Instance.onGetTileHandlers.Invoke();
        }

        IEnumerator ScanIt()
        {
            yield return null;
            //InputSignals.Instance.onDisableTouch?.Invoke();
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    TileHandler currentTile = _tileHandlers[x, y];

                    if (currentTile.IsChecked == false && currentTile.CurrentEntity.EntityType != EntitiesEnum.Bomb && currentTile.CurrentEntity.EntityType != EntitiesEnum.Rocket && currentTile.CurrentEntity.EntityType != EntitiesEnum.Box)
                    {
                        List<List<int>> list = currentTile.CurrentEntity.CheckForCombos();

                        //ilgili sekere ait eslesen sekerlerin listesi olustu
                        if (list.Count >= 5)//bomb
                        {
                            foreach (var candies in list)
                            {
                                _tileHandlers[candies[0], candies[1]].SpriteRenderer.color = Color.blue;
                                _tileHandlers[candies[0], candies[1]].IsBomb = true;
                                _tileHandlers[candies[0], candies[1]].IsRocket = false;
                            }
                        }
                        else if (list.Count >= 3)//rocket
                        {
                            foreach (var candies in list)
                            {
                                _tileHandlers[candies[0], candies[1]].SpriteRenderer.color = Color.red;
                                _tileHandlers[candies[0], candies[1]].IsBomb = false;
                                _tileHandlers[candies[0], candies[1]].IsRocket = true;
                            }
                        }

                        else
                        {
                            foreach (var candies in list)
                            {
                                //CandiesLocations[candies[0], candies[1]].GetComponent<AbsEntity>().IsChecked = false;
                                _tileHandlers[candies[0], candies[1]].GetComponent<SpriteRenderer>().color = Color.white;
                                _tileHandlers[candies[0], candies[1]].IsBomb = false;
                                _tileHandlers[candies[0], candies[1]].IsRocket = false;

                            }
                        }
                    }
                    else if (currentTile.IsChecked == false && (currentTile.CurrentEntity.EntityType == EntitiesEnum.Bomb || currentTile.CurrentEntity.EntityType == EntitiesEnum.Rocket))
                    {
                        currentTile.IsChecked = true;
                        currentTile.SpriteRenderer.color = Color.white;
                        currentTile.IsBomb = false;
                        currentTile.IsRocket = false;
                    }
                }
            }
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    _tileHandlers[x, y].IsChecked = false;
                }
            }
            //InputSignals.Instance.onEnableTouch?.Invoke(0);
        }

        public void ScanForCombo()
        {
            StartCoroutine(ScanIt());
        }
    }
}