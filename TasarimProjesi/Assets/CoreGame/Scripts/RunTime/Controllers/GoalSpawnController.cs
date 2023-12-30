using RunTime.Enums;
using RunTime.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace RunTime.Controllers
{
    public class GoalSpawnController : MonoBehaviour
    {
        [SerializeField] Transform goalContainer;
        [SerializeField] GameObject goalPrefab;
        [SerializeField] GoalCheckController goalCheckController;

        public void SpawnNewGoal(Sprite goalImg, EntitiesEnum goalType, int goalAmount)
        {
            GoalHandler newGoal = Instantiate(goalPrefab, goalContainer).GetComponent<GoalHandler>();
            newGoal.GoalImg = goalImg;
            newGoal.GoalAmount = goalAmount;
            newGoal.GoalType = goalType;
            goalCheckController.GoalHandlers.Add(newGoal);
        }
    }
}