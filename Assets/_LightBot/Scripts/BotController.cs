using System.Collections;
using System.Collections.Generic;
using LightBot.Commands;
using LightBot.Map;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private GridMapSO _gridMapSO;
    private List<BaseCommand> _commands;
    
    void Start()
    {
        _commands = new List<BaseCommand>();
        _commands.Add(new MoveCommand());
        _commands.Add(new RotateLeftCommand());
        _commands.Add(new MoveCommand());
        _commands.Add(new JumpMoveCommand());
        _commands.Add(new LightCommand());
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log($"running Move Command");
            var command = new MoveCommand();
            command.Run(transform, _gridMapSO);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log($"running Rotate Right Command");
            var command = new RotateRightCommand();
            command.Run(transform, _gridMapSO);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Debug.Log($"running Rotate Left Command");
            var command = new RotateLeftCommand();
            command.Run(transform, _gridMapSO);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log($"running Jump Command");
            var command = new JumpMoveCommand();
            command.Run(transform, _gridMapSO);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            RunProgram();
        }
    }
    
#endif

    public void RunProgram()
    {
        StartCoroutine(RunCommands());
    }

    private IEnumerator RunCommands()
    {
        for (int i = 0; i < _commands.Count; i++)
        {
            if (_commands[i].Run(transform, _gridMapSO))
                Debug.Log($"Command({i}) Yay! :D");
            else
                Debug.Log($"Command({i}) Nay X(");
            
            yield return new WaitForSeconds(1);
        }
        yield break;
    }
    
}
