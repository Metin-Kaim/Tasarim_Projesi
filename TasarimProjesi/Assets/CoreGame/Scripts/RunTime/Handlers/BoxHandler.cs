using RunTime.Abstracts.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Handlers
{
    public class BoxHandler : AbsEntity, ITouchable
    {
        [SerializeField] private TileHandler _currentTile;
        [SerializeField] private GameObject _child;

        public GameObject Child { get => _child; set => _child = value; }
        public TileHandler CurrentTile { get => _currentTile; set => _currentTile = value; }

        public void CallForCheck()
        {
            throw new System.NotImplementedException();
        }

        public void CheckForMatches(int x, int y, List<List<int>> chosenCandies)
        {
            throw new System.NotImplementedException();
        }

        public void CheckOtherDirections(int x, int y, List<List<int>> chosenCandies)
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseDown()
        {
            print("Box");
        }
    }
}