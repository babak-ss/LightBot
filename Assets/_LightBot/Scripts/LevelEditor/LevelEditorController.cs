using System;
using LightBot.Core;
using LightBot.Map;
using UnityEngine;

namespace LightBot.LevelEditor
{
    public class LevelEditorController : MonoBehaviour
    {
        public float theZ;
        [SerializeField] private Vector3EventSO _clickEvent;
        [SerializeField] private Vector3EventSO _longPressEvent;
        [SerializeField] private Camera _camera;
        [SerializeField] private GridMapSO _gridMapSO;
        [SerializeField] private Vector3EventSO _tileClickedEvent;
        [SerializeField] private Vector3EventSO _tileLongPressEvent;

        void Start()
        {
            _clickEvent?.Subscribe(OnClickEventListener);
            _longPressEvent?.Subscribe(OnLongPressEventListener);
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
                _tileLongPressEvent.RaiseEvent(tilePosition);
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
                _tileClickedEvent.RaiseEvent(tilePosition);
        }
    }
}
