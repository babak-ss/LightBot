using LightBot.Core;
using UnityEngine;

namespace LightBot.Commands
{
    public class CommandPrefabController : MonoBehaviour
    {
        [SerializeField] private VoidEventSO _refreshProgramViewEvent;
        private ProgramSO _program;
        private BaseCommand _selfCommand;

        public void Initialize(BaseCommand command, ProgramSO program)
        {
            _selfCommand = command;
            _program = program;
        }

        public void OnCommandClickButton()
        {
            RemoveCommandFromProgram();
        }

        private void RemoveCommandFromProgram()
        {
            _program.Commands.Remove(_selfCommand);
            _refreshProgramViewEvent.Raise();
        }
    }
}