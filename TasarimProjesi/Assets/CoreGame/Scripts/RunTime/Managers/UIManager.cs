using RunTime.Controllers;
using RunTime.Signals;
using UnityEngine;

namespace RunTime.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GoalSpawnController goalSpawnController;
        [SerializeField] GoalCheckController goalCheckController;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onSpawnNewGoal += goalSpawnController.SpawnNewGoal;
            UISignals.Instance.onCheckGoals += goalCheckController.CheckAllGoals;
        }
        private void UnSubscribeEvents()
        {
            UISignals.Instance.onSpawnNewGoal -= goalSpawnController.SpawnNewGoal;
            UISignals.Instance.onCheckGoals -= goalCheckController.CheckAllGoals;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

    }
}