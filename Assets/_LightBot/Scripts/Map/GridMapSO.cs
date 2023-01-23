using UnityEditor;
using UnityEngine;

namespace LightBot.Map
{
    [CreateAssetMenu(menuName = "Map/GridMap")]
    public class GridMapSO : ScriptableObject
    {
        [SerializeField] private GridMap _gridMap;
        
        [SerializeField] private int _levelNumber = 0;

        public GridMapSO()
        {
            Debug.Log("GridMapSO Constructor");
        }

        public void InitializeGridMapSO(int width, int height)
        {
            Debug.Log("Initialize GridMap");
            _gridMap = new GridMap(width, height);
        }
        
        public int getTilesCount() { return _gridMap.tiles.Length; }
        public int GetHeight() { return _gridMap.Height; }
        public int GetWidth() { return _gridMap.Width; }

        public int GetTileStep(int x, int y) { return GetTile(x, y).Step; }
        
        public Tile GetTile(int x, int y)
        {
            if (!CheckIsValid(x, y))
            {
                Debug.LogError($"Tile[{x}, {y}] IS NOT VALID!");
                return null;
            }
            
            return _gridMap.tiles[(x * GetHeight()) + y];
        }

        public Vector3 GetWorldPositionOfTile(int x, int y)
        {
            Vector3 worldPosition = new Vector3(GridMap.BASE_POSITION.x + GridMap.TILE_SIZE * x,
                                                GridMap.BASE_POSITION.y + GetTileStep(x, y) / 4f,
                                                GridMap.BASE_POSITION.z + GridMap.TILE_SIZE * y);
            return worldPosition;
        }

        public Vector3 GetLocalScaleOfTile(int x, int y)
        {
            Vector3 localScale = new Vector3(1, GetTileStep(x, y) / 2f + 0.5f, 1);
            return localScale;
        }

        public void SetTileStep(int x, int y, int step)
        {
            _gridMap.tiles[CalculateTileIndex(x, y)].Step = step;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
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