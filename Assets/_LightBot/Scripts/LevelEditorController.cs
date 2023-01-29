using LightBot.Commands;
using LightBot.Core;
using LightBot.Map;
using UnityEngine;
using Utilities;

public class LevelEditorController : MonoBehaviour
{
    [SerializeField] private VoidEventSO _runProgramEvent;
    [SerializeField] private VoidEventSO _resetBotEvent;
    [SerializeField] private VoidEventSO _resetProgramEvent;
    
    [SerializeField] private GridMapSO _currentGridMap;
    [SerializeField] private GameObject _botPrefab;
    [SerializeField] private Program _currentCommands;
    [SerializeField] private ObjectPoolSO _objectPool;
    private GameObject _botGameObject;
    
    void Start()
    {
        _runProgramEvent.Subscribe(RunProgram);
        _resetBotEvent.Subscribe(ResetBot);
        _resetProgramEvent.Subscribe(ResetProgram);
    }

    void Update()
    {
        
    }

    private void ResetBot()
    {
        if (_botGameObject == null)
            _botGameObject = _objectPool.Get(_botPrefab);
        _botGameObject = _objectPool.Get(_botPrefab);
        _botGameObject.transform.position = _currentGridMap.GetWorldPositionOfTile(0, 0);
        _botGameObject.transform.localEulerAngles = Vector3.zero;
    }

    private void ResetProgram()
    {
        _currentCommands.Instantiate();
    }

    private void RunProgram()
    {
        
    }
}
