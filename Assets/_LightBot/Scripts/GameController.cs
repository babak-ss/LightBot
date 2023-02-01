using System;
using LightBot.Core;
using UnityEditor;
using UnityEngine;

namespace LightBot
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController;
        [SerializeField] private GameDataSO _gameDataSO;

        [SerializeField] private VoidEventSO _backLevelButtonEvent;

        [SerializeField] private GameObject _gameMenuCanvas;
    
        void Start()
        {
            _backLevelButtonEvent.Subscribe(OnBackLevelButtonEventListener);
        }

        private void OnBackLevelButtonEventListener()
        {
            _gameMenuCanvas.SetActive(true);
            _levelController.gameObject.SetActive(false);
        }

        void Update()
        {
        
        }

        public void StartLevel(int levelIndex)
        {
            _gameMenuCanvas.SetActive(false);
            _levelController.gameObject.SetActive(true);
            _levelController.LoadLevelData(_gameDataSO.Levels[levelIndex]);
        }

        private void OnApplicationQuit()
        {
            EditorUtility.SetDirty(_gameDataSO);
        }
    }
}
