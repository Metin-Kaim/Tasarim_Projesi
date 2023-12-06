using RunTime.Abstracts.Entities;
using RunTime.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Handlers
{
    public class RocketHandler : AbsEntity, ITouchable
    {
        [SerializeField] private TileHandler _currentTile;

        public TileHandler CurrentTile { get => _currentTile; set => _currentTile = value; }

        bool _isRow;

        private void Awake()
        {
            float value = Random.value;

            if (value < .5f)
            {
                _isRow = true;
                transform.rotation = Quaternion.Euler(0, 0, -40);
            }
            else
            {
                _isRow = false;
                transform.rotation = Quaternion.Euler(0, 0, 50);
            }
        }

        public void OnMouseDown()
        {
            CallForCheck();
        }

        public void CheckForMatches(int x, int y, List<List<int>> chosenCandies)
        {

            if (y <= gridSize.y - 1 && y >= 0)//check for bounds
            {
                if (x <= gridSize.x - 1 && x >= 0)//check for bounds
                {
                    TileHandler otherTile = tileHandlersArray[x, y];
                    if (otherTile != null)
                    {
                        if (otherTile.GetComponent<TileHandler>().IsChecked != true)
                        {
                            //        if (otherTile.CurrentEntity.EntityType == EntityType)
                            //        {
                            //print($"x: {x} - {otherTile.CurrentEntity.EntityType}, y: {y} -  {EntityType}");
                            otherTile.IsChecked = true;//bir sonraki sekerin degerleri degistiriliyor.

                            chosenCandies.Add(new List<int> { x, y }); // eslesen her sekeri listeye ekleme.
                            if (otherTile.CurrentEntity.EntityType == EntitiesEnum.Bomb || otherTile.CurrentEntity.EntityType == EntitiesEnum.Rocket)
                                otherTile.CurrentEntity.GetComponent<ITouchable>().CheckOtherDirections(x, y, chosenCandies);
                            //        }
                        }
                    }
                }
            }
        }

        public void CheckOtherDirections(int x, int y, List<List<int>> chosenCandies)
        {
            if (_isRow)
            {
                for (int i = 0; i < gridSize.y; i++)
                {
                    CheckForMatches(x, i, chosenCandies);
                }
            }
            else
            {
                for (int i = 0; i < gridSize.x; i++)
                {
                    CheckForMatches(i, y, chosenCandies);
                }
            }
        }

        public void CallForCheck() //Herhangi bir sekilde sekerleri kontrol etmek ve ayni turde olan sekerler uzerinde islem yapmak icin gerekli adimlari bulunduran method
        {
            //Listeyi temizle
            _chosenCandies.Clear();

            CurrentTile.IsChecked = true;//secilen sekeri isaretle


            CheckOtherDirections(Row, Column, _chosenCandies);

            _chosenCandies.Add(new List<int> { Row, Column }); // secilen ilk sekerin koordinatlarýný al

            foreach (var item in _chosenCandies)
            {
                Destroy(tileHandlersArray[item[0], item[1]].CurrentEntity.gameObject);
            }

            //StartCoroutine(DestroyThem());
        }

        IEnumerator DestroyThem()
        {
            foreach (var item in _chosenCandies)
            {
                Destroy(tileHandlersArray[item[0], item[1]].CurrentEntity.gameObject);
                yield return new WaitForSeconds(0);
            }
        }

    }
}