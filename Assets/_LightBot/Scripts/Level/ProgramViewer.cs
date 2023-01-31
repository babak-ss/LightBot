using System;
using System.Collections.Generic;
using LightBot.Commands;
using LightBot.Core;
using UnityEngine;
using UnityEngine.UI;
using Utilities;


namespace LightBot
{
    public class ProgramViewer : MonoBehaviour
    {
        private ProgramSO _programSO;

        [SerializeField] private VoidEventSO _refreshProgramViewEvent;

        [SerializeField] private ObjectPoolSO _objectPool;
        [SerializeField] private GameObject _commandPrefab;
        [SerializeField] private Texture _moveCommandImage;
        [SerializeField] private Texture _rotateLeftCommandImage;
        [SerializeField] private Texture _rotateRightCommandImage;
        [SerializeField] private Texture _jumpCommandImage;
        [SerializeField] private Texture _lightCommandImage;

        // private Vector3 _commandBasePosition = new Vector3(60, 670, 0);
        private Vector3 _commandBasePosition = new Vector3(0, 0, 0);

        private List<GameObject> commandImagesList;

        public void LoadData(ProgramSO programSO)
        {
            _programSO = programSO;
        }
        
        private void OnEnable()
        {
            _refreshProgramViewEvent.Subscribe(OnRefreshViewProgramEventListener);
            commandImagesList = new List<GameObject>();
            DrawProgramView();
        }

        private void OnDisable()
        {
            _refreshProgramViewEvent.Unsubscribe(OnRefreshViewProgramEventListener);
        }

        private void OnRefreshViewProgramEventListener()
        {
            DrawProgramView();
        }

        private void DrawProgramView()
        {
            for (int i = 0; i < commandImagesList.Count; i++)
                _objectPool.Remove(commandImagesList[i]);
            commandImagesList = new List<GameObject>();
            GameObject commandGameObject;
            int index = 0;
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

                commandGameObject.GetComponent<CommandPrefabController>().SetSelfCommandAndProgram(command, _programSO);
                commandImagesList.Add(commandGameObject);
                commandGameObject.SetActive(true);

                index++;
            }
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.V))
            {
                DrawProgramView();
            }
        }
    }
}
