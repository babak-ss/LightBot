using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class JumpMoveCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            var tempPosition = transform.position + transform.right * 1;
            var destinationTile = gridMap.GetTileFromWorldPosition(tempPosition);
            transform.position = tempPosition + transform.up * 0.25f * destinationTile.Step;

            return true;
        }
    }
}