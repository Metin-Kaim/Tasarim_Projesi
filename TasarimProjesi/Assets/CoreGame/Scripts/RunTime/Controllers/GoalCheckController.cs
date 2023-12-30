using RunTime.Handlers;
using RunTime.Signals;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RunTime.Controllers
{
    public class GoalCheckController : MonoBehaviour
    {
        public List<GoalHandler> GoalHandlers = new();

        private TileHandler[,] _tileHandlers;

        private void Start()
        {
            _tileHandlers = GridSignals.Instance.onGetTileHandlers.Invoke();
        }

        public bool CheckAllGoals(List<List<int>> chosenCandies)
        {
            foreach (List<int> candy in chosenCandies)
            {
                for (int i = 0; i < GoalHandlers.Count; i++)
                {
                    if (_tileHandlers[candy[0], candy[1]].CurrentEntity.EntityType == GoalHandlers[i].GoalType)
                    {
                        GoalHandlers[i].GoalAmount--;
                    }
                }
            }

            bool isLevelDone = true;

            for (int i = 0; i < GoalHandlers.Count; i++)
            {
                if (GoalHandlers[i].GoalAmount > 0)
                {
                    isLevelDone = false;
                    break;
                }
            }
            if(isLevelDone)
            {
                UISignals.Instance.onOpenWinPanel?.Invoke();
            }

            return isLevelDone;
        }
    }
}