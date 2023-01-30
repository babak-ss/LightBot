using LightBot.Core;
using UnityEngine;

namespace LightBot
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private VoidEventSO _runProgramEvent;
        [SerializeField] private VoidEventSO _resetBotEvent;
        [SerializeField] private VoidEventSO _resetLevelButtonEvent;
        [SerializeField] private VoidEventSO _backLevelButtonEvent;
        
        [SerializeField] private VoidEventSO _jumpCommandButtonEvent;
        [SerializeField] private VoidEventSO _rotateLeftCommandButtonEvent;
        [SerializeField] private VoidEventSO _moveCommandButtonEvent;
        [SerializeField] private VoidEventSO _rotateRightCommandButtonEvent;
        [SerializeField] private VoidEventSO _lightCommandButtonEvent;
        
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
            _runProgramEvent.Raise();
            // _resetBotEvent.Raise();
        }
        

        public void JumpCommandButton()
        {
            _jumpCommandButtonEvent.Raise();
        }

        public void RotateLeftCommandButton()
        {
            _rotateLeftCommandButtonEvent.Raise();
        }

        public void MoveCommandButton()
        {
            _moveCommandButtonEvent.Raise();
        }

        public void RotateRightCommandButton()
        {
            _rotateRightCommandButtonEvent.Raise();
        }

        public void LightCommandButton()
        {
            _lightCommandButtonEvent.Raise();
        }
    }
}