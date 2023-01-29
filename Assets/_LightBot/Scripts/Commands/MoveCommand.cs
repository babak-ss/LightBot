using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class MoveCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            transform.position = transform.position + transform.right * 1;
            return true;
        }
    }
}