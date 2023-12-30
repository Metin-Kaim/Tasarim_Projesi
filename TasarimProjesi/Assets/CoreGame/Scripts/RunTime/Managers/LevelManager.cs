using UnityEngine;
using RunTime.Signals;
using RunTime.Datas.UnityObjects;

namespace RunTime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private readonly string _pp_currentLevel = "CurrentLevel";

        public int CurrentLevelIndex { get => PlayerPrefs.GetInt(_pp_currentLevel); set { PlayerPrefs.SetInt(_pp_currentLevel, value); } }

        private void Awake()
        {
            if (!PlayerPrefs.HasKey(_pp_currentLevel))
                PlayerPrefs.SetInt(_pp_currentLevel, 1);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            LevelSignals.Instance.onGetCurrentLevel += OnGetCurrentLevel;
            LevelSignals.Instance.onGetCurrentLevelIndex += () => CurrentLevelIndex;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onFailLevel += OnFailLevel;
        }

        private void OnFailLevel()
        {
            Debug.Log("Level Failed");
        }

        private void OnNextLevel()
        {
            //CurrentLevelIndex++;
            Debug.Log("Level Succeded");
        }

        private CD_Level OnGetCurrentLevel()
        {
            return Resources.Load<CD_Level>($"Levels/Level {CurrentLevelIndex}");
        }

        private void UnSubscribeEvents()
        {
            LevelSignals.Instance.onGetCurrentLevel -= OnGetCurrentLevel;
            LevelSignals.Instance.onGetCurrentLevelIndex -= () => CurrentLevelIndex;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onFailLevel -= OnFailLevel;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

    }
}