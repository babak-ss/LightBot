using System;
using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class JumpMoveCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            var currentTile = gridMap.GetTileFromWorldPosition(transform.position);
            if (currentTile == null)
                return false;
            
            var simpleMovePosition = transform.position + transform.right * 1;
            var destinationTile = gridMap.GetTileFromWorldPosition(simpleMovePosition);
            int stepGap = destinationTile.Step - currentTile.Step;
            if (destinationTile == null || stepGap == 0 || Math.Abs(stepGap) > 1)
                return false;
            
            transform.position = simpleMovePosition + transform.up * 0.25f * stepGap ;
            return true;

        }
    }
}