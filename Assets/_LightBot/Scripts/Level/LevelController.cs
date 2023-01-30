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
        private LevelSO _levelSO;
        private GridMapSO _currentGridMap;
        private ProgramSO _currentProgram;
        
        [SerializeField] private GameObject _botPrefab;
        private GameObject _botGameObject;

        [Header("Events")] 
        [SerializeField] private VoidEventSO _runProgramEvent;
        [SerializeField] private VoidEventSO _resetBotEvent;
        [SerializeField] private VoidEventSO _resetLevelEvent;
        [SerializeField] private VoidEventSO _refreshProgramViewEvent;
        [SerializeField] private VoidEventSO _refreshViewEvent;

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
            _currentProgram.Commands.Add(new RotateRightCommand());
            _currentProgram.Commands.Add(new JumpMoveCommand());
            _currentProgram.Commands.Add(new LightCommand());
        }
        
        private void OnEnable()
        {
            _runProgramEvent.Subscribe(RunProgram);
            _resetBotEvent.Subscribe(ResetBot);
            _resetLevelEvent.Subscribe(ResetLevel);

            _refreshViewEvent.Raise();
            ResetLevel();
        }

        private void OnDisable()
        {
            _runProgramEvent.Unsubscribe(RunProgram);
            _resetBotEvent.Unsubscribe(ResetBot);
            _resetLevelEvent.Unsubscribe(ResetLevel);
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
            _botGameObject.transform.SetParent(transform);
            _botGameObject.SetActive(true);
        }

        private void ResetLevel()
        {
            ResetBot();
            _refreshProgramViewEvent.Raise();
        }

        private void RunProgram()
        {
            StartCoroutine(RunCommands());
        }
        
        private IEnumerator RunCommands()
        {
            foreach (var command in _currentProgram.Commands)
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