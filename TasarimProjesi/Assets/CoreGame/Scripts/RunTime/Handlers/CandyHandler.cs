using DG.Tweening;
using RunTime.Abstracts.Entities;
using RunTime.Managers;
using RunTime.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Handlers
{
    public class CandyHandler : AbsEntity, ITouchable
    {
        [SerializeField] private TileHandler _currentTile;
        [SerializeField] private GameObject _child;
        public bool IsChoosed;
        public GameObject Child { get => _child; set => _child = value; }
        public TileHandler CurrentTile { get => _currentTile; set => _currentTile = value; }

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
                            if (otherTile.CurrentEntity.EntityType == EntityType)
                            {
                                //print($"x: {x} - {otherTile.CurrentEntity.EntityType}, y: {y} -  {EntityType}");
                                otherTile.IsChecked = true;//bir sonraki sekerin degerleri degistiriliyor.
                                otherTile.CurrentEntity.GetComponent<CandyHandler>().IsChoosed = true;
                                chosenCandies.Add(new List<int> { x, y }); // eslesen her sekeri listeye ekleme.

                                otherTile.CurrentEntity.GetComponent<ITouchable>().CheckOtherDirections(x, y, chosenCandies);
                            }
                        }
                    }
                }
            }
        }

        public void CheckOtherDirections(int x, int y, List<List<int>> chosenCandies)
        {
            CheckForMatches(x, y + 1, chosenCandies);
            CheckForMatches(x, y - 1, chosenCandies);
            CheckForMatches(x + 1, y, chosenCandies);
            CheckForMatches(x - 1, y, chosenCandies);
        }

        public void CallForCheck() //Herhangi bir sekilde sekerleri kontrol etmek ve ayni turde olan sekerler uzerinde islem yapmak icin gerekli adimlari bulunduran method
        {
            //Listeyi temizle
            _chosenCandies.Clear();

            CurrentTile.IsChecked = true;//secilen sekeri isaretle

            _chosenCandies.Add(new List<int> { Row, Column }); // secilen ilk sekerin koordinatlarýný al
            IsChoosed = true;

            CheckOtherDirections(Row, Column, _chosenCandies);

            if (_chosenCandies.Count > 1)
                foreach (var item in _chosenCandies)
                {
                    //GridSignals.Instance.onSpawnNewEntity?.Invoke(tileHandlersArray[item[0], item[1]]);
                    //Destroy(tileHandlersArray[item[0], item[1]].CurrentEntity.gameObject);
                    tileHandlersArray[item[0], item[1]].CurrentEntity.AdjustStateOfEntity(false);
                    tileHandlersArray[item[0], item[1]].CurrentEntity = null;
                    //if (tileHandlersArray[item[0], item[1]].CurrentEntity.GetComponent<CandyHandler>().IsChoosed)
                    //{

                    //}
                }

            IsCheckCleaner();
            FallUpperCandies();
            SpawnNewCandies();

        }
        private void FallUpperCandies()
        {
            foreach (var candies in _chosenCandies)
            {
                int sayac = 0;
                //Patlatýlan sekerin ustune yeni bir seker gelmis fakat candies henuz bu sekere erisememis ise bu sekeri pas gec. Mesela ust uste 2 seker olsun. Alttaki seker 1, ustteki seker 2 olsun. Eger 1. sekere once basildiysa 1. seker ustte bulunan tum sekerleri asagi indirecek. Boylelike hem kendi yeri hem de 2. sekerin yeri dolabilecek. Eger 2. sekerin yeri 1. seker araciligiyla dolmussa 2. seker tekrar ustundeki sekerleri dusurme islemi yapmayacak. "!!! Alttaki if icin gecerlidir. !!!"
                if (tileHandlersArray[candies[0], candies[1]].CurrentEntity == null)
                {
                    for (int i = 0; i < candies[0]; i++)// secilen sekerin ustundeki tum sekerlere tek tek erisim
                    {
                        AbsEntity upperCandy = tileHandlersArray[candies[0] - (i + 1), candies[1]].CurrentEntity;//bir ust sekere erisim.
                        if (upperCandy != null)
                        {
                            //upperCandy.InstantiateCandy(candies[0], candies[1] + i - 1 - sayac);
                            //upperCandy.CandyCanFall = true;
                            tileHandlersArray[candies[0] - (i + 1), candies[1]].CurrentEntity = null;

                            tileHandlersArray[upperCandy.Row + 1 + sayac, candies[1]].CurrentEntity = upperCandy;
                            upperCandy.GetComponent<ITouchable>().CurrentTile = tileHandlersArray[upperCandy.Row + 1 + sayac, candies[1]];

                            upperCandy.Row = upperCandy.Row + 1 + sayac;
                            upperCandy.transform.SetParent(upperCandy.GetComponent<ITouchable>().CurrentTile.transform);

                            upperCandy.transform.DOLocalMove(Vector3.zero, .5f);

                            //for (int j = sayac; j >= 0; j--)
                            //{
                            //    GridSignals.Instance.onSpawnNewEntity?.Invoke(tileHandlersArray[j, candies[1]]);
                            //}
                        }
                        else
                        {
                            sayac++;
                        }
                    }
                }
            }
        }

        private void SpawnNewCandies()
        {
            foreach (var candies in _chosenCandies)
            {
                for (int i = 0; i < candies[0]+ 1; i++)// secilen sekerin ustundeki tum sekerlere tek tek erisim
                {
                    AbsEntity upperCandy = tileHandlersArray[candies[0] - i, candies[1]].CurrentEntity;//bir ust sekere erisim. !Once kendi yerine bakiyor. 

                    if (upperCandy == null)
                    {
                        GridSignals.Instance.onSpawnNewEntity?.Invoke(tileHandlersArray[candies[0] - i, candies[1]]);
                        break;
                    }
                }
            }
        }
        public void IsCheckCleaner()
        {
            for (int y = 0; y < gridSize.x; y++)
            {
                for (int x = 0; x < gridSize.y; x++)
                {
                    tileHandlersArray[x, y].IsChecked = false;
                }
            }
        }
    }
}
