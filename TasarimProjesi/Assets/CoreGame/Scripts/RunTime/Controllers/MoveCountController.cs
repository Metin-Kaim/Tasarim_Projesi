using RunTime.Signals;
using TMPro;
using UnityEngine;

namespace RunTime.Controllers
{
    public class MoveCountController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI moveCountText;

        private int _moveCount;

        private void Start()
        {
            _moveCount = (int)LevelSignals.Instance.onGetCurrentLevel?.Invoke().LevelFeatures.LevelMoveCount.MoveCount;

            UpdateMoveCountText();
        }

        public bool AdjustMoveCount(int amount, bool isWin)
        {
            _moveCount += amount;

            UpdateMoveCountText();

            if (_moveCount <= 0 && !isWin)
            {
                UISignals.Instance.onOpenFailPanel?.Invoke();
                return true;
            }

            return false;
        }
        private void UpdateMoveCountText()
        {
            moveCountText.text = _moveCount.ToString();
        }
    }
}