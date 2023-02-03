using System.Collections;
using System.Collections.Generic;
using LightBot.Commands;
using LightBot.Events;
using LightBot.Map;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace LightBot.Level
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private ObjectPoolSO _objectPool;
        [SerializeField] private GameObject _botPrefab;
        [Header("Events")] 
        [SerializeField] private BoolEventSO _levelStateChangeEvent;
        [SerializeField] private VoidEventSO _resetLevelButtonEvent;
        [SerializeField] private VoidEventSO _refreshProgramViewEvent;
        [SerializeField] private CommandEventSO _commandButtonEvent;
        [SerializeField] private VoidEventSO _clearProgramEvent;
        [SerializeField] private LevelDataEventSO _levelDataEvent;
        [SerializeField] private VoidEventSO _levelWonEvent;
        
        private LevelSO _currentLevel;
        private GameObject _botGameObject;
        private bool _isProgramRunning = false;
        
        private void OnEnable()
        {
            _isProgramRunning = false;
            _levelDataEvent.Subscribe(OnLevelDataEventListener);
            _levelStateChangeEvent.Subscribe(OnLevelStateChangeEventListener);
            _resetLevelButtonEvent.Subscribe(OnResetLevelButtonEventListener);
            _commandButtonEvent.Subscribe(OnCommandButtonEventListener);
            _clearProgramEvent.Subscribe(OnClearProgramEventListener);
        }
        
        private void OnDisable()
        {
            _isProgramRunning = false;
            _levelDataEvent.Unsubscribe(OnLevelDataEventListener);
            _levelStateChangeEvent.Unsubscribe(OnLevelStateChangeEventListener);
            _resetLevelButtonEvent.Unsubscribe(OnResetLevelButtonEventListener);
            _commandButtonEvent.Unsubscribe(OnCommandButtonEventListener);
            _clearProgramEvent.Unsubscribe(OnClearProgramEventListener);
        }

        private void OnLevelDataEventListener(LevelSO level)
        {
            _currentLevel = level;
            ResetBot();
            _currentLevel.ProgramSO.Commands ??= new List<BaseCommand>();
        }

        private void OnLevelStateChangeEventListener(bool isRunning)
        {
            _isProgramRunning = isRunning;
            ResetBot();
            if (_isProgramRunning)
                RunProgram();
        }

        private void OnResetLevelButtonEventListener()
        {
            _clearProgramEvent.Raise();
            _levelStateChangeEvent.Raise(false);
        }

        private void OnCommandButtonEventListener(BaseCommand command)
        {
            _currentLevel.ProgramSO.Commands.Add(command);
            _refreshProgramViewEvent.Raise();
        }

        private void OnClearProgramEventListener()
        {
            _currentLevel.ProgramSO.Commands = new List<BaseCommand>();
            _refreshProgramViewEvent.Raise();
        }

        private void RunProgram()
        {
            StartCoroutine(RunCommands());
        }
        
        private void ResetBot()
        {
            if (_botGameObject != null)
                _objectPool.Remove(_botGameObject);
            _botGameObject = _objectPool.Get(_botPrefab);
            _botGameObject.transform.position = _currentLevel.GridMapSO.GetWorldPositionOfTile(0, 0);
            _botGameObject.transform.localEulerAngles = Vector3.zero;
            _botGameObject.transform.SetParent(transform);
            _botGameObject.SetActive(true);
        }
        
        private IEnumerator RunCommands()
        {
            List<Tile> lampTiles = _currentLevel.GridMapSO.GetLampTiles();
            foreach (var command in _currentLevel.ProgramSO.Commands)
            {
                if (command.Run(_botGameObject.transform, _currentLevel.GridMapSO))
                {
                    // Debug.Log($"running Command({command}) Yay! :D");
                    if (command.GetType() == typeof(LightCommand))
                    {
                        for (int i = 0; i < lampTiles.Count; i++)
                        {
                            if (lampTiles[i] == _currentLevel.GridMapSO.GetTileFromWorldPosition(_botGameObject.transform.position))
                            {
                                lampTiles.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                // else
                // {
                //     Debug.LogWarning($"running Command({command}) Nay X(");   
                // }
                
                float commandDelayTimer = 0;
                yield return new WaitUntil(() =>
                {
                    commandDelayTimer += Time.deltaTime;
                    return commandDelayTimer > 1 || _isProgramRunning == false;
                });

                if (_isProgramRunning == false)
                    yield break;
            }
            
                
            if (lampTiles.Count == 0)
                _levelWonEvent.Raise();
            
            _levelStateChangeEvent.Raise(false);
        }
    }
}