using DG.Tweening;
using RunTime.Abstracts.Entities;
using RunTime.Enums;
using RunTime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Handlers
{
    public class BombHandler : AbsEntity
    {
        public override void CheckOtherDirections(int x, int y, List<List<int>> chosenCandies)
        {
            CheckForMatches(x, y + 1, chosenCandies);
            CheckForMatches(x, y - 1, chosenCandies);
            CheckForMatches(x + 1, y, chosenCandies);
            CheckForMatches(x - 1, y, chosenCandies);
            CheckForMatches(x - 1, y - 1, chosenCandies);
            CheckForMatches(x - 1, y + 1, chosenCandies);
            CheckForMatches(x + 1, y - 1, chosenCandies);
            CheckForMatches(x + 1, y + 1, chosenCandies);
        }
    }
}