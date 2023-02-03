using LightBot.Commands;
using LightBot.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LightBot
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private Button _actionButton;
        
        [Header("Events")]
        [SerializeField] private VoidEventSO _resetLevelButtonEvent;
        [SerializeField] private VoidEventSO _backLevelButtonEvent;
        [SerializeField] private CommandEventSO _commandButtonEvent;
        [SerializeField] private BoolEventSO _levelStateChangeEvent;

        private bool _isProgramRunning = false;

        private void OnEnable()
        {
            _isProgramRunning = false;
            _levelStateChangeEvent.Subscribe(OnLevelStateChangeEventListener);
        }

        private void OnDisable()
        {
            _levelStateChangeEvent.Unsubscribe(OnLevelStateChangeEventListener);
        }
        
        private void OnLevelStateChangeEventListener(bool isRunning)
        {
            _isProgramRunning = isRunning;
            ChangeLevelUI(_isProgramRunning);
        }

        public void ResetLevelButton()
        {
            _resetLevelButtonEvent.Raise();
        }

        public void BackLevelButton()
        {
            _backLevelButtonEvent.Raise();
        }
        
        public void LevelActionButton()
        {
            _isProgramRunning = !_isProgramRunning;
            _levelStateChangeEvent.Raise(_isProgramRunning);
            ChangeLevelUI(_isProgramRunning);
        }
        
        public void JumpCommandButton()
        {
            if (_isProgramRunning)
                return;
            _commandButtonEvent.Raise(new JumpMoveCommand());
        }

        public void RotateLeftCommandButton()
        {
            if (_isProgramRunning)
                return;
            _commandButtonEvent.Raise(new RotateLeftCommand());
        }

        public void MoveCommandButton()
        {
            if (_isProgramRunning)
                return;
            _commandButtonEvent.Raise(new MoveCommand());
        }

        public void RotateRightCommandButton()
        {
            if (_isProgramRunning)
                return;
            _commandButtonEvent.Raise(new RotateRightCommand());
        }

        public void LightCommandButton()
        {
            if (_isProgramRunning)
                return;
            _commandButtonEvent.Raise(new LightCommand());
        }

        private void ChangeLevelUI(bool isRunning)
        {
            if (isRunning)
            {
                _actionButton.image.color = Color.red;
                _actionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop";
            }
            else
            {
                _actionButton.image.color = Color.green;
                _actionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Run";
            }
        }
    }
}