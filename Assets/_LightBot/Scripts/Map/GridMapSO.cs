using System;
using System.Collections.Generic;
using UnityEngine;

namespace LightBot.Map
{
    [CreateAssetMenu(menuName = "Level/GridMapSO")]
    public class GridMapSO : ScriptableObject
    {
        [SerializeField] private GridMap _gridMap;

        public void InitializeGridMapSO(int width, int height)
        {
            Debug.Log("Initialize GridMapSO");
            _gridMap = new GridMap(width, height);
        }
        
        public int getTilesCount() { return _gridMap.tiles.Length; }
        public int GetHeight() { return _gridMap.Height; }
        public int GetWidth() { return _gridMap.Width; }
        
        public Tile GetTile(int x, int y)
        {
            if (!CheckIsValid(x, y))
            {
                Debug.LogError($"Tile[{x}, {y}] IS NOT VALID!");
                return null;
            }
            
            return _gridMap.tiles[(x * GetHeight()) + y];
        }

        public int GetTileStep(int x, int y) { return GetTile(x, y).Step; }
        
        public bool IsLamp(int x, int y) { return GetTile(x, y).IsLamp; }

        public Vector3[] GetLampTilesPosition()
        {
            List<Vector3> lampTiles = new List<Vector3>();

            for (int i = 0; i < _gridMap.tiles.Length; i++)
            {
                if (_gridMap.tiles[i].IsLamp)
                    lampTiles.Add(new Vector3(_gridMap.tiles[i].X, _gridMap.tiles[i].Y, -1));
            }

            return lampTiles.ToArray();
        }

        public Vector3 GetWorldPositionOfTile(int x, int y)
        {
            Vector3 worldPosition = new Vector3(GridMap.BASE_POSITION.x + GridMap.TILE_SIZE * x,
                                                GridMap.BASE_POSITION.y + GetTileStep(x, y) / 4f,
                                                GridMap.BASE_POSITION.z + GridMap.TILE_SIZE * y);
            return worldPosition;
        }

        public Tile GetTileFromWorldPosition(Vector3 worldPosition)
        {
            Vector3 tilePosition = GetTilePositionFromWorldPosition(worldPosition);
            return GetTile((int)tilePosition.x, (int)tilePosition.y);
        }
        
        public Vector3 GetTilePositionFromWorldPosition(Vector3 worldPosition)
        {
            for (int i = 0; i < GetWidth(); i++)
            {
                if (GridMap.TILE_SIZE / 2 > 
                    Math.Abs(GetWorldPositionOfTile(i, 0).x - worldPosition.x))
                {
                    for (int j = 0; j < GetHeight(); j++)
                    {
                        if (GridMap.TILE_SIZE / 2 >
                            Math.Abs(GetWorldPositionOfTile(i, j).z - worldPosition.z))
                        {
                            // var clickedTile = GetTile(i, j);
                            // Debug.Log(clickedTile.ToString());
                            return new Vector3(i, j, -1);
                        }
                    }
                }
            }

            return Vector3.negativeInfinity;
        }

        public Vector3 GetLocalScaleOfTile(int x, int y)
        {
            Vector3 localScale = new Vector3(1, GetTileStep(x, y) / 2f + 0.5f, 1);
            return localScale;
        }

        public void SetTileStep(int x, int y, int step) { _gridMap.tiles[CalculateTileIndex(x, y)].Step = step; }

        public void SetTileIsLamp(int x, int y, bool isLamp)
        {
            _gridMap.tiles[CalculateTileIndex(x, y)].IsLamp = isLamp;
            Debug.Log($"##### setting tile[{x}, {y}] a lamp!");
        }
        
        public bool CheckIsValid(int x, int y) => x >= 0 && y >= 0 && 
                                                  x < GetWidth() & y < GetHeight() && 
                                                  CalculateTileIndex(x, y) < _gridMap.tiles.Length;
        
        private int CalculateTileIndex(int x, int y) => x * _gridMap.Height + y;

        public bool HasData()
        {
            return _gridMap.tiles != null;
        }
    }
}