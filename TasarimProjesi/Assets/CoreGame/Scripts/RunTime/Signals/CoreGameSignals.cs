using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class CoreGameSignals : MonoBehaviour
    {
        #region Singleton
        public static CoreGameSignals Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        #endregion

        public UnityAction onNextLevel;
        public UnityAction onFailLevel;
    }
}