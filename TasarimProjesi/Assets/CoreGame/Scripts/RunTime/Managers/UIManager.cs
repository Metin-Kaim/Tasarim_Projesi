using RunTime.Controllers;
using RunTime.Signals;
using UnityEngine;

namespace RunTime.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GoalSpawnController goalSpawnController;
        [SerializeField] GoalCheckController goalCheckController;
        [SerializeField] UIController uiController;
        [SerializeField] MoveCountController moveCountController;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onSpawnNewGoal += goalSpawnController.SpawnNewGoal;
            UISignals.Instance.onCheckGoals += goalCheckController.CheckAllGoals;
            UISignals.Instance.onOpenWinPanel += uiController.OpenWinPanel;
            UISignals.Instance.onOpenFailPanel += uiController.OpenFailPanel;
            UISignals.Instance.onAdjustMoveCount += moveCountController.AdjustMoveCount;
        }
        private void UnSubscribeEvents()
        {
            UISignals.Instance.onSpawnNewGoal -= goalSpawnController.SpawnNewGoal;
            UISignals.Instance.onCheckGoals -= goalCheckController.CheckAllGoals;
            UISignals.Instance.onOpenWinPanel -= uiController.OpenWinPanel;
            UISignals.Instance.onOpenFailPanel -= uiController.OpenFailPanel;
            UISignals.Instance.onAdjustMoveCount -= moveCountController.AdjustMoveCount;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

    }
}