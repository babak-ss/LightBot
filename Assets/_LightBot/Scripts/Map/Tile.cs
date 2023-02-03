using System;

namespace LightBot.Map
{
    [Serializable]
    public class Tile
    {
        public int x;
        public int y;
        public int Step;
        public bool IsLamp;

        public Tile(int x, int step, int y, bool isLamp)
        {
            this.x = x;
            this.y = y;
            Step = step;
            IsLamp = isLamp;
        }

        
        public override bool Equals(object obj) => this.Equals(obj as Tile);
        
        public bool Equals(Tile other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.GetType() != other.GetType()) return false;
            return x == other.x && y == other.y && Step == other.Step && IsLamp == other.IsLamp;
        }
        
        public override int GetHashCode() => (X: x, Y: y, step: Step, isLamp: IsLamp).GetHashCode();
        
        public static bool operator ==(Tile lhs, Tile rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                    return true;
                return false;
            }
            
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(Tile lhs, Tile rhs) => !(lhs == rhs);  
        
        public override string ToString()
        {
            return $"Tile[x={x}, y={y}, step={Step}, isLamp = {IsLamp}]";
        }
    }
}