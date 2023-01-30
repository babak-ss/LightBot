using System.Collections;
using LightBot.Commands;
using LightBot.Core;
using LightBot.Map;
using UnityEngine;
using Utilities;

namespace LightBot
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private ObjectPoolSO _objectPool;
        [SerializeField] private GridMapSO _currentGridMap;
        [SerializeField] private GameObject _botPrefab;
        private GameObject _botGameObject;

        [SerializeField] private Program _program;

        [Header("Events")] 
        [SerializeField] private VoidEventSO _runProgramEvent;
        [SerializeField] private VoidEventSO _resetBotEvent;
        [SerializeField] private VoidEventSO _resetLevelEvent;
        [SerializeField] private VoidEventSO _refreshProgramViewEvent;


        void Start()
        {
            
            _program.Instantiate();
            _program.Commands.Add(new MoveCommand());
            _program.Commands.Add(new RotateLeftCommand());
            _program.Commands.Add(new MoveCommand());
            _program.Commands.Add(new JumpMoveCommand());
            _program.Commands.Add(new LightCommand());
            _program.Commands.Add(new MoveCommand());
            _program.Commands.Add(new RotateLeftCommand());
            _program.Commands.Add(new MoveCommand());
            _program.Commands.Add(new JumpMoveCommand());
            _program.Commands.Add(new LightCommand());
            
            
            _runProgramEvent.Subscribe(RunProgram);
            _resetBotEvent.Subscribe(ResetBot);
            _resetLevelEvent.Subscribe(ResetLevel);

            ResetBot();
        }

        
#if UNITY_EDITOR
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                Debug.Log($"running Move Command");
                var command = new MoveCommand();
                command.Run(_botGameObject.transform, _currentGridMap);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log($"running Rotate Right Command");
                var command = new RotateRightCommand();
                command.Run(_botGameObject.transform, _currentGridMap);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log($"running Rotate Left Command");
                var command = new RotateLeftCommand();
                command.Run(_botGameObject.transform, _currentGridMap);
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                Debug.Log($"running Jump Command");
                var command = new JumpMoveCommand();
                command.Run(_botGameObject.transform, _currentGridMap);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                RunProgram();
            }
        }
#endif
        
        private void ResetBot()
        {
            if (_botGameObject != null)
                _objectPool.Remove(_botGameObject);
            _botGameObject = _objectPool.Get(_botPrefab);
            _botGameObject.transform.position = _currentGridMap.GetWorldPositionOfTile(0, 0);
            _botGameObject.transform.localEulerAngles = Vector3.zero;
            _botGameObject.SetActive(true);
        }

        private void ResetLevel()
        {
            _program.Instantiate();
            ResetBot();
            _refreshProgramViewEvent.Raise();
        }

        private void RunProgram()
        {
            StartCoroutine(RunCommands());
        }
        
        public IEnumerator RunCommands()
        {
            foreach (var command in _program.Commands)
            {
                yield return new WaitForSeconds(1);
                
                if (command.Run(_botGameObject.transform, _currentGridMap))
                    Debug.Log($"Command({command}) Yay! :D");
                else
                    Debug.Log($"Command({command}) Nay X(");
            }

            yield break;
        }
    }
}