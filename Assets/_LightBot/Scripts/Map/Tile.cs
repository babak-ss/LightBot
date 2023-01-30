using System;

namespace LightBot.Map
{
    [Serializable]
    public class Tile
    {
        public int X;
        public int Y;
        public int Step;
        public bool IsLamp;

        public Tile(int x, int step, int y, bool isLamp)
        {
            X = x;
            Y = y;
            Step = step;
            IsLamp = isLamp;
        }

        public override string ToString()
        {
            return $"Tile[x={X}, y={Y}, step={Step}, isLamp = {IsLamp}]";
        }
    }
}