using LightBot.Map;
using UnityEngine;

namespace LightBot.Commands
{
    public abstract class BaseCommand
    {
        public abstract bool Run(Transform transform, GridMapSO gridMap);
    }
}