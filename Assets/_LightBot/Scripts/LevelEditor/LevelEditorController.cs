using System;
using LightBot.Core;
using LightBot.Map;
using UnityEngine;

namespace LightBot.LevelEditor
{
    public class LevelEditorController : MonoBehaviour
    {
        public float theZ;
        [SerializeField] private Camera _camera;
        
        [SerializeField] private GridMapSO _gridMapSO;
        [SerializeField] private int _width = 5;
        [SerializeField] private int _height = 5 ;
        
        [SerializeField] private VoidEventSO _initializeMapButtonEvent;
        [SerializeField] private VoidEventSO _refreshViewButtonEvent;
        
        [SerializeField] private Vector3EventSO _clickEvent;
        [SerializeField] private Vector3EventSO _longPressEvent;
        
        [SerializeField] private Vector3EventSO _tileClickedEvent;
        [SerializeField] private Vector3EventSO _tileLongPressEvent;
        
        [SerializeField] private VoidEventSO _refreshViewEvent;

        void Start()
        {
            _initializeMapButtonEvent?.Subscribe(OnInitializeMapEventListener);
            _refreshViewButtonEvent?.Subscribe(OnRefreshViewButtonEventListener);
            
            _clickEvent?.Subscribe(OnClickEventListener);
            _longPressEvent?.Subscribe(OnLongPressEventListener);
            
            _tileClickedEvent?.Subscribe(OnTileClickedEventListener);
            _tileLongPressEvent?.Subscribe(OnTileLongPressEventListener);
        }

        private void OnLongPressEventListener(Vector3 clickedPosition)
        {
            if (!_gridMapSO.HasData())
            {
                Debug.Log("No GridMap!");
                return;
            }
            
            Vector3 clickWorldPosition = _camera.ScreenToWorldPoint(new Vector3(clickedPosition.x, clickedPosition.y, theZ));
            Vector3 tilePosition = _gridMapSO.GetTilePositionFromWorldPosition(clickWorldPosition);
            
            if (_gridMapSO.CheckIsValid((int)tilePosition.x, (int)tilePosition.y))
                _tileLongPressEvent.Raise(tilePosition);
        }

        private void OnClickEventListener(Vector3 clickedPosition)
        {
            if (!_gridMapSO.HasData())
            {
                Debug.Log("No GridMap!");
                return;
            }
            
            Vector3 clickWorldPosition = _camera.ScreenToWorldPoint(new Vector3(clickedPosition.x, clickedPosition.y, theZ));
            Vector3 tilePosition = _gridMapSO.GetTilePositionFromWorldPosition(clickWorldPosition);
            
            if (_gridMapSO.CheckIsValid((int)tilePosition.x, (int)tilePosition.y))
                _tileClickedEvent.Raise(tilePosition);
        }
        
        private void OnTileClickedEventListener(Vector3 t)
        {
            // Debug.Log($"found tile: {t}");
            int tilePreviousStep = _gridMapSO.GetTileStep((int)t.x, (int)t.y);
            if (tilePreviousStep > 3)
                tilePreviousStep = -1;
            _gridMapSO.SetTileStep((int)t.x, (int)t.y, tilePreviousStep + 1);
            _refreshViewEvent.Raise();
        }
        
        private void OnTileLongPressEventListener(Vector3 t)
        {
            // Debug.Log($"found tile: {t}");
            bool isLamp = _gridMapSO.IsLamp((int)t.x, (int)t.y);
            _gridMapSO.SetTileIsLamp((int)t.x, (int)t.y, !isLamp);
            _refreshViewEvent.Raise();
        }

        private void OnRefreshViewButtonEventListener()
        {
            _refreshViewEvent.Raise();
        }

        private void OnInitializeMapEventListener()
        {
            _gridMapSO.InitializeGridMapSO(_width, _height);
            _refreshViewEvent.Raise();
        }
    }
}
