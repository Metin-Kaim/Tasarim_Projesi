using RunTime.Enums;
using System;
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
        public Func<List<List<int>>, bool> onCheckGoals;
        public UnityAction onOpenWinPanel;
        public UnityAction onOpenFailPanel;
        public Func<int, bool, bool> onAdjustMoveCount;
    }
}