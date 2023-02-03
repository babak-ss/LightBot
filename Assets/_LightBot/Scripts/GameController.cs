using LightBot.Events;
using LightBot.Level;
using UnityEditor;
using UnityEngine;

namespace LightBot
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController;
        [SerializeField] private GameDataSO _gameDataSO;
        [SerializeField] private GameObject _gameMenuCanvas;

        [SerializeField] private VoidEventSO _backLevelButtonEvent;
        [SerializeField] private LevelDataEventSO _levelDataEvent;
    
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

        public void StartLevel(int level)
        {
            _gameMenuCanvas.SetActive(false);
            _levelController.gameObject.SetActive(true);
            _levelDataEvent.Raise(_gameDataSO.Levels[level - 1]);
        }

        private void OnApplicationQuit()
        {
            EditorUtility.SetDirty(_gameDataSO);
        }
    }
}
