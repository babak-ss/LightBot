using System.Collections.Generic;
using LightBot.Commands;
using UnityEngine;

namespace LightBot
{
    [CreateAssetMenu(menuName = "Level/Program")]
    public class ProgramSO : ScriptableObject
    {
        public List<BaseCommand> Commands;
    }
}