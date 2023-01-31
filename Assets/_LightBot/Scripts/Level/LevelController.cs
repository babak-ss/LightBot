using System.Collections;
using System.Collections.Generic;
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
        private LevelSO _levelSO;
        private GridMapSO _currentGridMap;
        private ProgramSO _currentProgram;
        private bool _isProgramRunning = false;
        
        [SerializeField] private GameObject _botPrefab;
        private GameObject _botGameObject;

        [Header("Events")] 
        [SerializeField] private BoolEventSO _levelStateChangeEvent;
        [SerializeField] private VoidEventSO _resetLevelEvent;
        [SerializeField] private VoidEventSO _refreshProgramViewEvent;
        [SerializeField] private VoidEventSO _refreshGridMapViewEvent;
        [SerializeField] private CommandEventSO _commandButtonEvent;
        [SerializeField] private VoidEventSO _clearProgramEvent;

        public void LoadLevelData(LevelSO levelSO)
        {
            _levelSO = levelSO;
            _currentGridMap = _levelSO.GridMapSO;
            _currentProgram = _levelSO.ProgramSO;
            
            //TODO: load this using a "level data event"
            GetComponentInChildren<GridMapViewer>().LoadData(_currentGridMap);
            GetComponentInChildren<ProgramViewer>().LoadData(_currentProgram);
            
            _currentProgram.Instantiate();
            _currentProgram.Commands.Add(new MoveCommand());
            _currentProgram.Commands.Add(new RotateLeftCommand());
            _currentProgram.Commands.Add(new MoveCommand());
            _currentProgram.Commands.Add(new LightCommand());
            _currentProgram.Commands.Add(new MoveCommand());
            _currentProgram.Commands.Add(new JumpMoveCommand());
            _currentProgram.Commands.Add(new JumpMoveCommand());
            _currentProgram.Commands.Add(new MoveCommand());
        }
        
        private void OnEnable()
        {
            _isProgramRunning = false;
            _levelStateChangeEvent.Subscribe(OnLevelStateChangeEventListener);
            _resetLevelEvent.Subscribe(OnResetLevelEventListener);
            _commandButtonEvent.Subscribe(OnCommandButtonEventListener);
            _clearProgramEvent.Subscribe(OnClearProgramEventListener);

            _refreshGridMapViewEvent.Raise();
            ResetBot();
        }
        
        private void OnDisable()
        {
            _levelStateChangeEvent.Unsubscribe(OnLevelStateChangeEventListener);
            _resetLevelEvent.Unsubscribe(OnResetLevelEventListener);
            _commandButtonEvent.Unsubscribe(OnCommandButtonEventListener);
            _clearProgramEvent.Unsubscribe(OnClearProgramEventListener);
        }

        private void OnLevelStateChangeEventListener(bool isRunning)
        {
            _isProgramRunning = isRunning;
            ResetBot();
            if (_isProgramRunning)
                RunProgram();
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

        private void RunProgram()
        {
            StartCoroutine(RunCommands());
        }
        
        private void ResetBot()
        {
            if (_botGameObject != null)
                _objectPool.Remove(_botGameObject);
            _botGameObject = _objectPool.Get(_botPrefab);
            _botGameObject.transform.position = _currentGridMap.GetWorldPositionOfTile(0, 0);
            _botGameObject.transform.localEulerAngles = Vector3.zero;
            _botGameObject.transform.SetParent(transform);
            _botGameObject.SetActive(true);
        }

        private void OnResetLevelEventListener()
        {
            _clearProgramEvent.Raise();
        }

        private void OnCommandButtonEventListener(BaseCommand command)
        {
            _currentProgram.Commands.Add(command);
            _refreshProgramViewEvent.Raise();
        }

        private void OnClearProgramEventListener()
        {
            _currentProgram.Commands = new List<BaseCommand>();
            _refreshProgramViewEvent.Raise();
        }
        
        private IEnumerator RunCommands()
        {
            foreach (var command in _currentProgram.Commands)
            {
                if (command.Run(_botGameObject.transform, _currentGridMap))
                    Debug.Log($"running Command({command}) Yay! :D");
                else
                    Debug.LogWarning($"running Command({command}) Nay X(");
                
                float commandDelayTimer = 0;
                yield return new WaitUntil(() =>
                {
                    commandDelayTimer += Time.deltaTime;
                    return commandDelayTimer > 1 || _isProgramRunning == false;
                });
                if (_isProgramRunning == false)
                    yield break;
            }
            
            _levelStateChangeEvent.Raise(false);
        }
    }
}