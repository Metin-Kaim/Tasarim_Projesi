﻿using RunTime.Signals;
using System;
using System.Collections;
using UnityEngine;

namespace RunTime.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static int TimerCounter = 0;

        [SerializeField] private bool isEnable = true;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onEnableTouch += OnEnableTouch;
            InputSignals.Instance.onDisableTouch += () => isEnable = false;
            InputSignals.Instance.onGetStateOfTouch += () => isEnable;
        }

        public void OnEnableTouch(float time)
        {
            StartCoroutine(EnableTouch(time));
        }
        IEnumerator EnableTouch(float time)
        {
            yield return new WaitForSeconds(0);
            isEnable = true;
        }

        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onEnableTouch -= OnEnableTouch;
            InputSignals.Instance.onDisableTouch -= () => isEnable = false;
            InputSignals.Instance.onGetStateOfTouch -= () => isEnable;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}