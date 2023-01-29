using System;
using LakeHorse;
using LightBot.Core;
using UnityEditor;
using UnityEngine;

namespace LightBot.Map
{
    public class GridMapController : MonoBehaviour
    {
        [SerializeField] private Vector3EventSO _tileClickedEvent;
        [SerializeField] private Vector3EventSO _tileLongPressEvent;
        [SerializeField] private VoidEventSO _initializeMapEvent;
        [SerializeField] private VoidEventSO _refreshGridMapViewEvent;
        [SerializeField] private GridMapSO _gridMapSO;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private ObjectPoolSO _objectPool;
        [SerializeField] private int _width = 5;
        [SerializeField] private int _height = 5;
        
        private GameObject[] _tiles;

        private void Start()
        {
            _tileClickedEvent?.Subscribe(OnTileClickedEventListener);
            _tileLongPressEvent?.Subscribe(OnTileLongPressEventListener);
            _initializeMapEvent?.Subscribe(OnInitializeMapEventListener);
            _refreshGridMapViewEvent?.Subscribe(OnRefreshGridMapViewEventListener);
        }
        
        private void OnTileClickedEventListener(Vector3 t)
        {
            Debug.Log($"found tile: {t}");
            int tilePreviousStep = _gridMapSO.GetTileStep((int)t.x, (int)t.y);
            if (tilePreviousStep > 3)
                tilePreviousStep = -1;
            _gridMapSO.SetTileStep((int)t.x, (int)t.y, tilePreviousStep + 1);
            DrawMap();
        }
        
        private void OnTileLongPressEventListener(Vector3 t)
        {
            Debug.Log($"found tile: {t}");
            bool isLamp = _gridMapSO.IsLamp((int)t.x, (int)t.y);
            _gridMapSO.SetTileIsLamp((int)t.x, (int)t.y, !isLamp);
            DrawMap();
        }

        private void OnRefreshGridMapViewEventListener()
        {
            DrawMap();
        }

        private void OnInitializeMapEventListener()
        {
            _gridMapSO.InitializeGridMapSO(_width, _height);
        }

        public void DrawMap()
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
            
            GameObject newTile;
            int counter = 0;
            for (int i = 0; i < _gridMapSO.GetWidth(); i++)
            {
                for (int j = 0; j < _gridMapSO.GetHeight(); j++)
                {
                    if (!_gridMapSO.CheckIsValid(i, j))
                        break;
                    
                    newTile = _objectPool.Get(_tilePrefab);
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

        // private void OnApplicationFocus(bool hasFocus)
        // {
        //     Debug.Log($"OnApplicationFocus: {hasFocus}");
        //     EditorUtility.SetDirty(_gridMapSO);
        //     AssetDatabase.SaveAssets();
        //     AssetDatabase.Refresh();
        // }
    }
}