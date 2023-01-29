using LightBot.Core;
using UnityEngine;

namespace LightBot
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private VoidEventSO _runProgramEvent;
        [SerializeField] private VoidEventSO _resetBotEvent;
        [SerializeField] private VoidEventSO _resetProgramEvent;

        public void RunProgramButton()
        {
            _runProgramEvent.Raise();
        }

        public void ResetBotButton()
        {
            _resetBotEvent.Raise();
        }

        public void ResetProgramButton()
        {
            _resetProgramEvent.Raise();
        }
    }
}