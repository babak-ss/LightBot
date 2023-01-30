using System.Collections.Generic;
using LightBot.Commands;
using UnityEngine;

namespace LightBot
{
    [CreateAssetMenu(menuName = "Level/Program")]
    public class ProgramSO : ScriptableObject
    {
        public List<BaseCommand> Commands;

        // public List<Methodes> Methodes;

        public void Instantiate()
        {
            Commands = new List<BaseCommand>();
        }
    }
}