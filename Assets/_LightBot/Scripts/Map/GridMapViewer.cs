using LightBot.Events;
using UnityEngine;
using Utilities;

namespace LightBot.Map
{
    public class GridMapViewer : MonoBehaviour
    {
        [SerializeField] private VoidEventSO _refreshGridMapViewEvent;
        [SerializeField] private LevelDataEventSO _levelDataEvent;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private ObjectPoolSO _objectPool;
        [SerializeField] private GridMapSO _gridMapSO;
        
        private GameObject[] _tiles;
        
        private void OnEnable()
        {
            Debug.Log("GridMapViewer OnEnable");
            _refreshGridMapViewEvent.Subscribe(OnRefreshGridMapViewEventListener);
            _levelDataEvent.Subscribe(OnLevelDataEventListener);
        }

        private void OnDisable()
        {
            _refreshGridMapViewEvent.Unsubscribe(OnRefreshGridMapViewEventListener);
        }

        private void OnRefreshGridMapViewEventListener()
        {
            DrawMap();
        }

        private void OnLevelDataEventListener(LevelSO level)
        {
            Debug.Log("GridMapViewer OnLevelDataEvent");
            LoadData(level.GridMapSO);
            DrawMap();
        }

        private void DrawMap()
        {
            if (_tiles != null)
            {
                for (int i = 0; i < _tiles.Length; i++)
                {
                    if (_tiles[i] != null)
                    {
                        _tiles[i].transform.localScale = Vector3.one;
                        _tiles[i].GetComponentInChildren<Renderer>().material.color = Color.white;
                        _objectPool.Remove(_tiles[i]);
                    }
                }
            }
            _tiles = new GameObject[_gridMapSO.getTilesCount()];
            
            int counter = 0;
            for (int i = 0; i < _gridMapSO.GetWidth(); i++)
            {
                for (int j = 0; j < _gridMapSO.GetHeight(); j++)
                {
                    Tile tile = _gridMapSO.GetTile(i, j);
                    if (tile == null)
                        break;
                    
                    GameObject newTile = _objectPool.Get(_tilePrefab);
                    newTile.transform.localPosition = _gridMapSO.GetWorldPositionOfTile(tile);
                    
                    if (_gridMapSO.GetTile(i, j).IsLamp)
                        newTile.GetComponentInChildren<Renderer>().material.color = Color.blue;

                    newTile.transform.parent = transform;
                    newTile.name = $"Tile[{i},{j}]";
                    _tiles[counter] = newTile;
                    counter++;
                    newTile.SetActive(true);
                }
            }
        }

        private void LoadData(GridMapSO gridMapSO)
        {
            _gridMapSO = gridMapSO;
        }
    }
}