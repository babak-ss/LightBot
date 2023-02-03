using System;
using System.Collections.Generic;
using LightBot.Commands;
using LightBot.Events;
using UnityEngine;
using UnityEngine.UI;
using Utilities;


namespace LightBot
{
    public class ProgramViewer : MonoBehaviour
    {
        private ProgramSO _programSO;

        [SerializeField] private VoidEventSO _refreshProgramViewEvent;
        [SerializeField] private LevelDataEventSO _levelDataEvent;

        [SerializeField] private ObjectPoolSO _objectPool;
        [SerializeField] private GameObject _commandPrefab;
        [SerializeField] private Texture _moveCommandImage;
        [SerializeField] private Texture _rotateLeftCommandImage;
        [SerializeField] private Texture _rotateRightCommandImage;
        [SerializeField] private Texture _jumpCommandImage;
        [SerializeField] private Texture _lightCommandImage;
        private List<GameObject> commandImagesList;
        
        private void OnEnable()
        {
            Debug.Log("ProgramViewer OnEnable");
            _refreshProgramViewEvent.Subscribe(OnRefreshViewProgramEventListener);
            _levelDataEvent.Subscribe(OnLevelDataEventListener);
        }

        private void OnDisable()
        {
            _refreshProgramViewEvent.Unsubscribe(OnRefreshViewProgramEventListener);
            _levelDataEvent.Unsubscribe(OnLevelDataEventListener);
        }

        private void OnRefreshViewProgramEventListener()
        {
            DrawProgramView();
        }

        private void OnLevelDataEventListener(LevelSO level)
        {
            Debug.Log("Program Viewer OnLevelDataEvent");
            LoadData(level.ProgramSO);
            DrawProgramView();
        }

        private void DrawProgramView()
        {
            if (commandImagesList != null)
                for (int i = 0; i < commandImagesList.Count; i++)
                    _objectPool.Remove(commandImagesList[i]);
            commandImagesList = new List<GameObject>();

            int index = 0;
            GameObject commandGameObject;
            foreach (var command in _programSO.Commands)
            {
                commandGameObject = _objectPool.Get(_commandPrefab);
                if (command.GetType() == typeof(MoveCommand))
                {
                    commandGameObject.GetComponent<RawImage>().texture = _moveCommandImage;
                }
                else if (command.GetType() == typeof(RotateLeftCommand))
                {
                    commandGameObject.GetComponent<RawImage>().texture = _rotateLeftCommandImage;
                }
                else if (command.GetType() == typeof(RotateRightCommand))
                {
                    commandGameObject.GetComponent<RawImage>().texture = _rotateRightCommandImage;
                }
                else if (command.GetType() == typeof(JumpMoveCommand))
                {
                    commandGameObject.GetComponent<RawImage>().texture = _jumpCommandImage;
                }
                else if (command.GetType() == typeof(LightCommand))
                {
                    commandGameObject.GetComponent<RawImage>().texture = _lightCommandImage;
                }
                else
                {
                    // commandGameObject.GetComponent<Image>().sprite = _moveCommandImage;
                }
                commandGameObject.transform.SetParent(transform.GetChild(0));

                commandGameObject.transform.position = new Vector3(60, Screen.height - 160, 0) + 
                                                      Vector3.down * ((index / 6) * 100) + 
                                                      Vector3.right * ((index % 6) * 100);

                // TODO: make event
                commandGameObject.GetComponent<CommandPrefabController>().Initialize(command, _programSO);
                commandImagesList.Add(commandGameObject);
                commandGameObject.SetActive(true);

                index++;
            }
        }

        private void LoadData(ProgramSO programSO)
        {
            _programSO = programSO;
            Debug.Log($"LoadData program viewer {programSO}");
            
        }
        
#if UNITY_EDITOR
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.V))
            {
                DrawProgramView();
            }
        }
#endif
    }
}
