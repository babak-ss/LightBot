using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class LightCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            return gridMap.GetTileFromWorldPosition(transform.position).IsLamp;
        }
    }
}