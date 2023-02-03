using System;
using System.Collections;
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
        
        public Tile GetTile(float x, float y) { return GetTile((int)x, (int)y); }
        
        public Tile GetTile(Vector3 position) { return GetTile(position.x, position.y); }

        public int GetTileStep(int x, int y) { return GetTile(x, y).Step; }

        public int GetTileStep(float x, float y) { return GetTile(x, y).Step; }
        
        public bool IsLamp(int x, int y) { return GetTile(x, y).IsLamp; }
        
        public bool IsLamp(float x, float y) { return GetTile(x, y).IsLamp; }
        
        public List<Tile> GetLampTiles()
        {
            List<Tile> lampTiles = new List<Tile>();
            foreach (var tile in _gridMap.tiles)
            {
                if (tile.IsLamp)
                    lampTiles.Add(tile);
                Debug.Log(tile);
            }

            return lampTiles;
        }

        public Vector3 GetWorldPositionOfTile(int x, int y)
        {
            Vector3 worldPosition = new Vector3(GridMap.BASE_POSITION.x + GridMap.TILE_SIZE * x,
                                                GridMap.BASE_POSITION.y + GetTileStep(x, y) / 4f,
                                                GridMap.BASE_POSITION.z + GridMap.TILE_SIZE * y);
            return worldPosition;
        }

        public Vector3 GetWorldPositionOfTile(Tile tile) => GetWorldPositionOfTile(tile.x, tile.y);

        public Tile GetTileFromWorldPosition(Vector3 worldPosition)
        {
            Vector3? tilePosition = GetTilePositionFromWorldPosition(worldPosition);
            if (tilePosition != null)
                return GetTile((Vector3)tilePosition);
            else
                return null;
        }
        
        public Vector3? GetTilePositionFromWorldPosition(Vector3 worldPosition)
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
                            return new Vector3(i, j, -1);
                        }
                    }
                }
            }

            return null;
        }

        public void SetTileStep(int x, int y, int step) { _gridMap.tiles[CalculateTileIndex(x, y)].Step = step; }
        
        public void SetTileStep(float x, float y, int step) => SetTileStep((int)x, (int)y, step);

        public void SetTileIsLamp(int x, int y, bool isLamp)
        {
            _gridMap.tiles[CalculateTileIndex(x, y)].IsLamp = isLamp;
        }

        public void SetTileIsLamp(float x, float y, bool isLamp) => SetTileIsLamp((int)x, (int)y, isLamp);
        
        private bool CheckIsValid(int x, int y) => x >= 0 && y >= 0 && 
                                                  x < GetWidth() & y < GetHeight() && 
                                                  CalculateTileIndex(x, y) < _gridMap.tiles.Length;

        private bool CheckIsValid(float x, float y) => CheckIsValid((int)x, (int)y);
        
        private int CalculateTileIndex(int x, int y) => x * _gridMap.Height + y;

        public bool HasData()
        {
            return _gridMap.tiles != null;
        }
    }
}