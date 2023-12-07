using DG.Tweening;
using RunTime.Abstracts.Entities;
using RunTime.Managers;
using RunTime.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Handlers
{
    public class CandyHandler : AbsEntity
    {
        protected override void CheckForMatches(int x, int y, List<List<int>> chosenCandies)
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

                                otherTile.CurrentEntity.CheckOtherDirections(x, y, chosenCandies);
                            }
                        }
                    }
                }
            }
        }

        public override void CheckOtherDirections(int x, int y, List<List<int>> chosenCandies)
        {
            CheckForMatches(x, y + 1, chosenCandies);
            CheckForMatches(x, y - 1, chosenCandies);
            CheckForMatches(x + 1, y, chosenCandies);
            CheckForMatches(x - 1, y, chosenCandies);
        }
        
    }
}
