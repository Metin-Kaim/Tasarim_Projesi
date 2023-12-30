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
            spriteRenderer.DOFade(0, 1).From().OnComplete(() =>
            {
                //yield return new WaitForSeconds(1);
                newPanel.GetComponentInChildren<Button>().onClick.AddListener(action);
            });
            yield return null;
        }
    }
}