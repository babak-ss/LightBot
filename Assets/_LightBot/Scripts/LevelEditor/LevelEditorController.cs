using System;
using LightBot.Core;
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
        
        [SerializeField] private VoidEventSO _initializeMapButtonEvent;
        [SerializeField] private VoidEventSO _refreshGridMapViewButtonEvent;

        [SerializeField] private LevelDataEventSO _levelDataEvent;
        
        [SerializeField] private Vector3EventSO _clickEvent;
        [SerializeField] private Vector3EventSO _longPressEvent;
        
        // [SerializeField] private Vector3EventSO _tileClickedEvent;
        // [SerializeField] private Vector3EventSO _tileLongPressEvent;
        
        [FormerlySerializedAs("_refreshViewEvent")] [SerializeField] private VoidEventSO _refreshGridMapViewEvent;

        void Start()
        {
            _initializeMapButtonEvent.Subscribe(OnInitializeMapEventListener);
            _refreshGridMapViewButtonEvent.Subscribe(OnRefreshViewButtonEventListener);
            
            _clickEvent.Subscribe(OnClickEventListener);
            _longPressEvent.Subscribe(OnLongPressEventListener);
            
            _levelDataEvent.Raise(_level);
        }
        
        private void OnLongPressEventListener(Vector3 clickedPosition)
        {
            Debug.Log("OnLongPressEventListener - LevelEditorController");
            if (!_level.GridMapSO.HasData())
            {
                Debug.Log("No GridMap!");
                return;
            }
            
            Vector3 clickWorldPosition = _camera.ScreenToWorldPoint(new Vector3(clickedPosition.x, clickedPosition.y, theZ));
            Vector3 tilePosition = _level.GridMapSO.GetTilePositionFromWorldPosition(clickWorldPosition);
            
            Debug.Log("figured clicked tile pos - long press - LevelEditorController");
            if (_level.GridMapSO.CheckIsValid((int)tilePosition.x, (int)tilePosition.y))
                OnTileLongPressEventListener(tilePosition);
        }

        private void OnClickEventListener(Vector3 clickedPosition)
        {
            Debug.Log("OnClickEventListener - LevelEditorController");
            if (!_level.GridMapSO.HasData())
            {
                Debug.Log("No GridMap!");
                return;
            }
            
            Vector3 clickWorldPosition = _camera.ScreenToWorldPoint(new Vector3(clickedPosition.x, clickedPosition.y, theZ));
            Vector3 tilePosition = _level.GridMapSO.GetTilePositionFromWorldPosition(clickWorldPosition);
            
            Debug.Log("figured clicked tile pos - LevelEditorController");
            
            if (_level.GridMapSO.CheckIsValid((int)tilePosition.x, (int)tilePosition.y))
                OnTileClickedEventListener(tilePosition);
        }
        
        private void OnTileClickedEventListener(Vector3 t)
        {
            // Debug.Log($"found tile: {t}");
            int tilePreviousStep = _level.GridMapSO.GetTileStep((int)t.x, (int)t.y);
            if (tilePreviousStep > 3)
                tilePreviousStep = -1;
            _level.GridMapSO.SetTileStep((int)t.x, (int)t.y, tilePreviousStep + 1);
            _refreshGridMapViewEvent.Raise();
        }
        
        private void OnTileLongPressEventListener(Vector3 t)
        {
            // Debug.Log($"found tile: {t}");
            bool isLamp = _level.GridMapSO.IsLamp((int)t.x, (int)t.y);
            _level.GridMapSO.SetTileIsLamp((int)t.x, (int)t.y, !isLamp);
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

        private void OnApplicationQuit()
        {
            Debug.Log("On App quit");
            UnityEditor.EditorUtility.SetDirty(_level);
        }
    }
}
