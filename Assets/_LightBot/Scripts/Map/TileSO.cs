using UnityEngine;

namespace LightBot.Map
{
    [CreateAssetMenu(menuName = "Map/Tile")]
    public class TileSO : ScriptableObject
    {
        [SerializeField] private Tile _tile;
        
        public int X
        {
            set
            {
                _tile.X = value;
            }
            get
            {
                return _tile.X;
            }
        }

        public int Step
        {
            set
            {
                _tile.Step = value;
            }
            get
            {
                return _tile.Step;
            }
        }

        public int Y
        {
            set
            {
                _tile.Y = value;
            }
            get
            {
                return _tile.Y;
            }
        }
    }
}