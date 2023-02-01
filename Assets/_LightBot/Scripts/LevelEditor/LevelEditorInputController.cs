using LightBot.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace LightBot.LevelEditor
{
    public class LevelEditorInputController : MonoBehaviour
    {
        [SerializeField] private Vector3EventSO _clickEvent;
        [SerializeField] private Vector3EventSO _longPressEvent;
        [SerializeField] private VoidEventSO _initializeMapButtonEvent;
        [SerializeField] private VoidEventSO _refreshGridMapViewButtonEvent;

        private bool isLongPress = false;
        private float pressTimer = 0;
        private const float LONG_PRESS_TIME = 1.5f;

        void Update()
        {
            if (isLongPress)
                pressTimer += Time.deltaTime;
            
            if (isLongPress == false && Input.GetMouseButtonDown(0))
                isLongPress = true;
            
            if (Input.GetMouseButtonUp(0))
            {
                if (pressTimer > LONG_PRESS_TIME)
                    _longPressEvent.Raise(Input.mousePosition);
                else
                    _clickEvent.Raise(Input.mousePosition);
                
                isLongPress = false;
                pressTimer = 0;
            }
        }

        public void OnInitializeMapButtonClicked()
        {
            _initializeMapButtonEvent.Raise();
        }

        public void OnRefreshViewMapButtonClicked()
        {
            _refreshGridMapViewButtonEvent.Raise();
        }
    }
}
