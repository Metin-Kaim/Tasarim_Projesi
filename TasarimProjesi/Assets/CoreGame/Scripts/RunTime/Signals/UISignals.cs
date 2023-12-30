using RunTime.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class UISignals : MonoBehaviour
    {
        #region Singleton
        public static UISignals Instance { get; private set; }

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

        public UnityAction<Sprite, EntitiesEnum, int> onSpawnNewGoal;
        public UnityAction<List<List<int>>> onCheckGoals;
    }
}