using RunTime.Handlers;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Abstracts.Entities
{
    internal interface ITouchable
    {
        public TileHandler CurrentTile { get; set; }
        void OnMouseDown();

        void CheckForMatches(int x, int y, List<List<int>> chosenCandies);
        void CheckOtherDirections(int x, int y, List<List<int>> chosenCandies);
        void CallForCheck();
    }
}
