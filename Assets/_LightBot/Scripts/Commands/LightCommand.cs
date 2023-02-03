using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class LightCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            if (gridMap.GetTileFromWorldPosition(transform.position).IsLamp)
            {
                transform.GetComponent<BotAnimationController>().StartLightAnimation(); //TODO: better way maybe event?
                return true;
            }

            return false;
        }
    }
}