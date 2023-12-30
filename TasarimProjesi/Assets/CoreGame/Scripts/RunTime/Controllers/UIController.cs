using DG.Tweening;
using RunTime.Signals;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RunTime.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject winPanel;
        [SerializeField] GameObject failPanel;
        [SerializeField] Transform gameCanvas;

        public void OpenWinPanel()
        {
            StartCoroutine(OpenPanels(winPanel, CoreGameSignals.Instance.onNextLevel));
        }
        public void OpenFailPanel()
        {
            StartCoroutine(OpenPanels(failPanel, CoreGameSignals.Instance.onFailLevel));
        }

        IEnumerator OpenPanels(GameObject panel, UnityAction action)
        {
            GameObject newPanel = Instantiate(panel, gameCanvas);
            Image spriteRenderer = newPanel.GetComponent<Image>();
            Button button = newPanel.GetComponentInChildren<Button>();
            button.interactable = false;
            spriteRenderer.DOFade(1, 1.5f).From(0).OnComplete(() =>
            {
                button.onClick.AddListener(action);
                button.interactable = true;
            });
            
            yield return null;
        }
    }
}