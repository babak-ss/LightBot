using LightBot.Commands;
using LightBot.Core;
using UnityEngine;
using Utilities;

public class ProgramViewer : MonoBehaviour
{
    [SerializeField] private Program _program;

    [SerializeField] private VoidEventSO _refreshProgramViewEvent;

    [SerializeField] private ObjectPoolSO _objectPool;
    [SerializeField] private GameObject _moveCommandPrefab;
    [SerializeField] private GameObject _rotateLeftCommandPrefab;
    [SerializeField] private GameObject _rotateRightCommandPrefab;
    [SerializeField] private GameObject _jumpCommandCommandPrefab;
    [SerializeField] private GameObject _lightCommandPrefab;
    [SerializeField] private GameObject _methodeCommandPrefab;

    private Vector3 _commandBasePosition = new Vector3(60, 770, 0);
    
    void Start()
    {
        _refreshProgramViewEvent?.Subscribe(OnRefreshViewProgramEventListener);
    }

    private void OnRefreshViewProgramEventListener()
    {
        DrawProgramView();
    }

    private void DrawProgramView()
    {
        GameObject commandImage;
        int index = 0;
        foreach (var command in _program.Commands)
        {
            if (command.GetType() == typeof(MoveCommand))
            {
                commandImage = _objectPool.Get(_moveCommandPrefab);
            }
            else if (command.GetType() == typeof(RotateLeftCommand))
            {
                commandImage = _objectPool.Get(_rotateLeftCommandPrefab);
            }
            else if (command.GetType() == typeof(RotateRightCommand))
            {
                commandImage = _objectPool.Get(_rotateRightCommandPrefab);
            }
            else if (command.GetType() == typeof(JumpMoveCommand))
            {
                commandImage = _objectPool.Get(_jumpCommandCommandPrefab);
            }
            else if (command.GetType() == typeof(LightCommand))
            {
                commandImage = _objectPool.Get(_lightCommandPrefab);
            }
            else
            {
                commandImage = _objectPool.Get(_methodeCommandPrefab);
            }
            
            commandImage.transform.localPosition = _commandBasePosition + Vector3.down * ((index / 6) * 100)
                                                                        + Vector3.right * ((index % 6) * 100);;
            commandImage.transform.parent = transform.GetChild(0);
            commandImage.SetActive(true);

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
