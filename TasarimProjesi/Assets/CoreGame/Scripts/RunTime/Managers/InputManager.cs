using RunTime.Signals;
using System;
using System.Collections;
using UnityEngine;

namespace RunTime.Managers
{
    public class InputManager : MonoBehaviour
    {
        private bool _isEnable = true;

        public static int TimerCounter = 0;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onEnableTouch += OnEnableTouch;
            InputSignals.Instance.onDisableTouch += () => _isEnable = true;
            InputSignals.Instance.onGetStateOfTouch += () => _isEnable;
        }

        public void OnEnableTouch(float time)
        {
            StartCoroutine(EnableTouch(time));
        }
        IEnumerator EnableTouch(float time)
        {
            yield return new WaitForSeconds(0);
            _isEnable = true;
        }

        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onEnableTouch -= OnEnableTouch;
            InputSignals.Instance.onDisableTouch -= () => _isEnable = false;
            InputSignals.Instance.onGetStateOfTouch -= () => _isEnable;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}