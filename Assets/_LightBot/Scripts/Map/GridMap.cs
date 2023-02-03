using System;
using UnityEngine;

namespace LightBot.Map
{
    [Serializable]
    public class GridMap
    {
        public int Width;
        public int Height;
        public Tile[] tiles;
        
        public static readonly Vector3 BASE_POSITION = Vector3.zero;
        public static readonly float TILE_SIZE = 1;

        public GridMap(int width, int height)
        {
            Width = width;
            Height = height;
            InitializeGridMap(width, height);
        }

        private void InitializeGridMap(int width, int height)
        {
            tiles = new Tile[width * height];
            int counter = 0;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    tiles[counter] = new Tile(i, 0, j, false);
                    counter++;
                }
            }
        }
    }
}
