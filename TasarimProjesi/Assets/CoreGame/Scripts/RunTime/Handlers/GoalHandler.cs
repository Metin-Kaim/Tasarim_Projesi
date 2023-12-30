using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RunTime.Enums;

namespace RunTime.Handlers
{
    public class GoalHandler : MonoBehaviour
    {

        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI goalAmountText;
        [SerializeField] EntitiesEnum goalType;

        [SerializeField] private int _goalAmount;
        [SerializeField] private Sprite goalImg;
        public int GoalAmount
        {
            get
            {
                return _goalAmount;
            }

            set
            {
                _goalAmount = value;
                if (_goalAmount <= 0)
                {
                    _goalAmount = 0;
                }
                goalAmountText.text = _goalAmount.ToString();
            }
        }

        public Sprite GoalImg
        {
            get
            {
                return goalImg;
            }
            set
            {
                goalImg = value;
                image.sprite = goalImg;
            }
        }

        public EntitiesEnum GoalType { get => goalType; set => goalType = value; }
    }
}
