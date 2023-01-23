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
        [SerializeField] private Camera _camera;
        [SerializeField] private GridMapSO _gridMapSO;
        [SerializeField] private Vector3EventSO _tileClickedEvent;
        private const float TILE_SIZE = 1;
        void Start()
        {
            _clickEvent?.Subscribe(OnClickEventListener);
        }
        
        private void OnClickEventListener(Vector3 clickedPosition)
        {
            if (!_gridMapSO.HasData())
            {
                Debug.Log("No GridMap!");
                return;
            }
            
            Vector3 clickWorldPosition = _camera.ScreenToWorldPoint(new Vector3(clickedPosition.x, clickedPosition.y, theZ));
            
            for (int i = 0; i < _gridMapSO.GetWidth(); i++)
            {
                if (GridMap.TILE_SIZE / 2 > 
                    Math.Abs(_gridMapSO.GetWorldPositionOfTile(i, 0).x - clickWorldPosition.x))
                {
                    for (int j = 0; j < _gridMapSO.GetHeight(); j++)
                    {
                        if (GridMap.TILE_SIZE / 2 >
                            Math.Abs(_gridMapSO.GetWorldPositionOfTile(i, j).z - clickWorldPosition.z))
                        {
                            var clickedTile = _gridMapSO.GetTile(i, j);
                            Debug.Log(clickedTile.ToString());
                            _tileClickedEvent.RaiseEvent(new Vector3(i, j, -1));
                            return;
                        }
                    }
                }
            }
            Debug.Log("NO TILES CLICKED!");
        }
    }
}
