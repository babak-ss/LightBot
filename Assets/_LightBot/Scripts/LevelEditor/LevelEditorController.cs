using System;
using LightBot.Events;
using LightBot.Level;
using LightBot.Map;
using UnityEngine;
using UnityEngine.Serialization;

namespace LightBot.LevelEditor
{
    public class LevelEditorController : MonoBehaviour
    {
        public float theZ;
        [SerializeField] private Camera _camera;
        
        [SerializeField] private LevelSO _level;
        [SerializeField] private int _width = 5;
        [SerializeField] private int _height = 5 ;
        [Header("Events")]
        [SerializeField] private VoidEventSO _initializeMapButtonEvent;
        [SerializeField] private VoidEventSO _refreshGridMapViewButtonEvent;
        [SerializeField] private LevelDataEventSO _levelDataEvent;
        [SerializeField] private Vector3EventSO _clickEvent;
        [SerializeField] private Vector3EventSO _longPressEvent;
        [SerializeField] private VoidEventSO _refreshGridMapViewEvent;

        void Start()
        {
            _initializeMapButtonEvent.Subscribe(OnInitializeMapEventListener);
            _refreshGridMapViewButtonEvent.Subscribe(OnRefreshViewButtonEventListener);
            _levelDataEvent.Raise(_level);
            _clickEvent.Subscribe(OnClickEventListener);
            _longPressEvent.Subscribe(OnLongPressEventListener);
        }
        
        private void OnLongPressEventListener(Vector3 clickedPosition)
        {
            if (!_level.GridMapSO.HasData())
            {
                Debug.Log("No GridMap!");
                return;
            }
            
            Vector3 clickWorldPosition = _camera.ScreenToWorldPoint(new Vector3(clickedPosition.x, clickedPosition.y, theZ));
            Tile tilePosition = _level.GridMapSO.GetTileFromWorldPosition(clickWorldPosition);
            
            if (tilePosition !=  null)
                OnTileLongPressEventListener(tilePosition);
        }

        private void OnClickEventListener(Vector3 clickedPosition)
        {
            if (!_level.GridMapSO.HasData())
            {
                Debug.Log("No GridMap!");
                return;
            }
            
            Vector3 clickWorldPosition = _camera.ScreenToWorldPoint(new Vector3(clickedPosition.x, clickedPosition.y, theZ));
            Tile tilePosition = _level.GridMapSO.GetTileFromWorldPosition(clickWorldPosition);
            
            if (tilePosition !=  null)
                OnTileClickedEventListener(tilePosition);
        }
        
        private void OnTileClickedEventListener(Tile tile)
        {
            int tilePreviousStep = tile.Step;
            if (tilePreviousStep > 3)
                tilePreviousStep = -1;
            _level.GridMapSO.SetTileStep(tile, tilePreviousStep + 1);
            _refreshGridMapViewEvent.Raise();
        }
        
        private void OnTileLongPressEventListener(Tile tile)
        {
            bool isLamp = tile.IsLamp;
            _level.GridMapSO.SetTileIsLamp(tile, !isLamp);
            _refreshGridMapViewEvent.Raise();
        }

        private void OnRefreshViewButtonEventListener()
        {
            _refreshGridMapViewEvent.Raise();
        }

        private void OnInitializeMapEventListener()
        {
            _level.GridMapSO.InitializeGridMapSO(_width, _height);
            _refreshGridMapViewEvent.Raise();
        }
    }
}
