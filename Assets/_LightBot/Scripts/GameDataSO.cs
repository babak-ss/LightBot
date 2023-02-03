using LightBot.Level;
using UnityEngine;

namespace LightBot
{
    [CreateAssetMenu(menuName = "GameDataSO")]
    public class GameDataSO : ScriptableObject
    {
        public LevelSO[] Levels;
    }
}