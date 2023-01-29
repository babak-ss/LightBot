using System.Collections;
using LightBot.Commands;
using LightBot.Map;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private GridMapSO _gridMapSO;
    [SerializeField] private Program program;
    
    void Start()
    {
        program.Instantiate();
        program.Commands.Add(new MoveCommand());
        program.Commands.Add(new RotateLeftCommand());
        program.Commands.Add(new MoveCommand());
        program.Commands.Add(new JumpMoveCommand());
        program.Commands.Add(new LightCommand());
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
        foreach (var command in program.Commands)
        {
            if (command.Run(transform, _gridMapSO))
                Debug.Log($"Command({command}) Yay! :D");
            else
                Debug.Log($"Command({command}) Nay X(");
            
            yield return new WaitForSeconds(1);
        }
        yield break;
    }
    
}
