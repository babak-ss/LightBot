using LightBot.Core;
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
            _levelController.LoadLevelData(_gameDataSO.Levels[levelIndex]);
            _gameMenuCanvas.SetActive(false);
            _levelController.gameObject.SetActive(true);
        }
    }
}
