using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class MoveCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            var currentTile = gridMap.GetTileFromWorldPosition(transform.position);
            if (currentTile == null)
                return false;
            
            Vector3 newPosition = transform.position + transform.right * 1;
            var destinationTile = gridMap.GetTileFromWorldPosition(newPosition);
            
            if (destinationTile == null || destinationTile.Step != currentTile.Step)
                return false;
            
            transform.position = newPosition;
            return true;
        }
    }
}