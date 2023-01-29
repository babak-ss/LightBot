using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class RotateLeftCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            transform.Rotate(0, -90, 0);

            return true;
        }
    }
}