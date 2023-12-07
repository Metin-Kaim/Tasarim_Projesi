using DG.Tweening;
using RunTime.Abstracts.Entities;
using RunTime.Enums;
using RunTime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Handlers
{
    public class RocketHandler : AbsEntity
    {
        bool _isRow;

        public override void Awake()
        {
            base.Awake();

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

        public override void CheckOtherDirections(int x, int y, List<List<int>> chosenCandies)
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
    }
}