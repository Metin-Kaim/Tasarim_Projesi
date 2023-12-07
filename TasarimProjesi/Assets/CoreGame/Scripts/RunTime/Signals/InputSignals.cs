using System;
using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class InputSignals : MonoBehaviour
    {
        #region Singleton
        public static InputSignals Instance { get; private set; }

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

        public UnityAction<float> onEnableTouch;
        public UnityAction onDisableTouch;
        public Func<bool> onGetStateOfTouch;
    }
}