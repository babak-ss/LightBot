using LightBot.Core;
using UnityEngine;
using Utilities;

namespace LightBot.Map
{
    public class GridMapViewer : MonoBehaviour
    {
        [SerializeField] private VoidEventSO _refreshViewEvent;
        [SerializeField] private GridMapSO _gridMapSO;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private ObjectPoolSO _objectPool;
        
        private GameObject[] _tiles;

        private void Start()
        {
            _refreshViewEvent?.Subscribe(OnRefreshViewEventListener);
        }

        private void OnRefreshViewEventListener()
        {
            DrawMap();
        }

        private void DrawMap()
        {
            if (_tiles == null)
                _tiles = new GameObject[_gridMapSO.getTilesCount()];
            else
                for (int i = 0; i < _tiles.Length; i++)
                {
                    if (_tiles[i] != null)
                    {
                        _tiles[i].transform.localScale = Vector3.one;
                        _tiles[i].GetComponentInChildren<Renderer>().material.color = Color.white;
                        _objectPool.Remove(_tiles[i]);
                    }
                }

            int counter = 0;
            for (int i = 0; i < _gridMapSO.GetWidth(); i++)
            {
                for (int j = 0; j < _gridMapSO.GetHeight(); j++)
                {
                    if (!_gridMapSO.CheckIsValid(i, j))
                        break;
                    
                    GameObject newTile = _objectPool.Get(_tilePrefab);
                    newTile.transform.localPosition = _gridMapSO.GetWorldPositionOfTile(i, j);
                    newTile.GetComponentInChildren<Transform>().localScale = _gridMapSO.GetLocalScaleOfTile(i, j);
                    
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
    }
}