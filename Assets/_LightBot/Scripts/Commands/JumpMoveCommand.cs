using System;
using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class JumpMoveCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            Vector3 botPosition = transform.position;
            Tile currentTile = gridMap.GetTileFromWorldPosition(botPosition);
            if (currentTile == null)
                return false;
            
            Vector3 simpleMovePosition = botPosition + transform.right * 1;
            Tile destinationTile = gridMap.GetTileFromWorldPosition(simpleMovePosition);
            int stepGap = destinationTile.Step - currentTile.Step;
            
            if (destinationTile == null || stepGap == 0 || Math.Abs(stepGap) > 1)
                return false;
            
            transform.position = simpleMovePosition + transform.up * 0.25f * stepGap ;
            return true;

        }
    }
}