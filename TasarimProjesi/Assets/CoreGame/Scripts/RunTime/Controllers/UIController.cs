using RunTime.Signals;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RunTime.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject winPanel;
        [SerializeField] Transform gameCanvas;

        public void OpenWinPanel()
        {
            GameObject winPanel = Instantiate(this.winPanel, gameCanvas);
            winPanel.GetComponentInChildren<Button>().onClick.AddListener(CoreGameSignals.Instance.onNextLevel.Invoke);
        }
    }
}