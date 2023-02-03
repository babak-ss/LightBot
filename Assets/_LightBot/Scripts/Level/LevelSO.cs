using LightBot.Map;
using UnityEngine;

namespace LightBot.Level
{
    [CreateAssetMenu(menuName = "Level/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        public int LevelNumber = 0;
        public GridMapSO GridMapSO;
        public ProgramSO ProgramSO;
    }
}