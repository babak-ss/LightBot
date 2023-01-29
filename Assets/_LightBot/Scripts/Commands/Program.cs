using System.Collections.Generic;
using UnityEngine;

namespace LightBot.Commands
{
    [CreateAssetMenu(menuName = "Program")]
    public class Program : ScriptableObject
    {
        [SerializeField] private int _levelNumber = 0;

        [SerializeField] public List<BaseCommand> Commands;

        // public List<Methodes> Methodes;

        public void Instantiate()
        {
            Commands = new List<BaseCommand>();
        }
    }
}