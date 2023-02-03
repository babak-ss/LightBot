using System;
using System.Collections;
using System.Collections.Generic;
using LightBot.Commands;
using LightBot.Events;
using LightBot.Map;
using UnityEngine;

namespace LightBot.Level
{
    public class ProgramRunner
    {
        private Transform _botTransform;
        private LevelSO _currentLevel;
        private BoolEventSO _levelStateChangeEvent;
        private VoidEventSO _levelWonEvent;

        private bool _isProgramRunning = false;

        public ProgramRunner(Transform botTransform, LevelSO currentLevel, BoolEventSO levelStateChangeEvent, VoidEventSO levelWonEvent)
        {
            _botTransform = botTransform;
            _currentLevel = currentLevel;
            _levelStateChangeEvent = levelStateChangeEvent;
            _levelStateChangeEvent.Subscribe(OnLevelStateChangeEvent);
            _levelWonEvent = levelWonEvent;
            _isProgramRunning = true;
        }

        private void OnLevelStateChangeEvent(bool isRunning)
        {
            _isProgramRunning = isRunning;
        }

        public IEnumerator RunCommands()
        {
            List<Tile> lampTiles = _currentLevel.GridMapSO.GetLampTiles();
            foreach (var command in _currentLevel.ProgramSO.Commands)
            {
                if (command.Run(_botTransform, _currentLevel.GridMapSO))
                {
                    // Debug.Log($"running Command({command}) Yay! :D");
                    if (command.GetType() == typeof(LightCommand))
                    {
                        for (int i = 0; i < lampTiles.Count; i++)
                        {
                            if (lampTiles[i] == _currentLevel.GridMapSO.GetTileFromWorldPosition(_botTransform.position))
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
                
                DateTime commandInitialTime = DateTime.UtcNow;
                yield return new WaitUntil(() => 
                    DateTime.UtcNow - commandInitialTime > TimeSpan.FromSeconds(1) || 
                    _isProgramRunning == false
                );

                if (_isProgramRunning == false)
                    yield break;
            }
            
                
            if (lampTiles.Count == 0)
                _levelWonEvent.Raise();
            
            _levelStateChangeEvent.Raise(false);
        }
    }
}