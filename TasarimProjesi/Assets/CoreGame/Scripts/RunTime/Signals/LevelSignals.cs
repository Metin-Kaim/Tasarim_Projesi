using RunTime.Datas.UnityObjects;
using System;
using UnityEngine;

namespace RunTime.Signals
{
    public class LevelSignals : MonoBehaviour
    {
        #region Singleton
        public static LevelSignals Instance { get; private set; }

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

        public Func<CD_Level> onGetCurrentLevel;
        public Func<int> onGetCurrentLevelIndex;

    }
}