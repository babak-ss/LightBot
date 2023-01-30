using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public class LightCommand : BaseCommand
    {
        public override bool Run(Transform transform, GridMapSO gridMap)
        {
            Debug.Log(gridMap.GetTileFromWorldPosition(transform.position));
            if (gridMap.GetTileFromWorldPosition(transform.position).IsLamp)
            {
                transform.GetComponent<BotAnimationController>().StartLightAnimation();
                return true;
            }

            return false;
        }
    }
}