using RunTime.Abstracts.Entities;
using RunTime.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Handlers
{
    public class CandyHandler : AbsEntity, ITouchable
    {
        [SerializeField] private TileHandler _currentTile;
        
        public TileHandler CurrentTile { get => _currentTile; set => _currentTile = value; }

        public void OnMouseDown()
        {
            CallForCheck();
        }

        private void CheckForMatches(int x, int y, List<List<int>> chosenCandies)
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

                                chosenCandies.Add(new List<int> { x, y }); // eslesen her sekeri listeye ekleme.

                                otherTile.CurrentEntity.GetComponent<CandyHandler>().CheckEveryDirections(x, y, chosenCandies);
                            }
                        }
                    }
                }
            }
        }

        private void CheckEveryDirections(int x, int y, List<List<int>> chosenCandies)
        {
            CheckForMatches(x, y + 1, chosenCandies);
            CheckForMatches(x, y - 1, chosenCandies);
            CheckForMatches(x + 1, y, chosenCandies);
            CheckForMatches(x - 1, y, chosenCandies);
        }

        public virtual void CallForCheck() //Herhangi bir sekilde sekerleri kontrol etmek ve ayni turde olan sekerler uzerinde islem yapmak icin gerekli adimlari bulunduran method
        {
            //Listeyi temizle
            GridManager._chosenCandies.Clear();

            CurrentTile.IsChecked = true;//secilen sekeri isaretle

            GridManager._chosenCandies.Add(new List<int> { Row,Column }); // secilen ilk sekerin koordinatlarýný al

            CheckEveryDirections(Row, Column, GridManager._chosenCandies);

            foreach (var item in GridManager._chosenCandies)
            {
                foreach (var c in item)
                {
                    print(c);
                }
                print("---");
            }
        }

    }
}