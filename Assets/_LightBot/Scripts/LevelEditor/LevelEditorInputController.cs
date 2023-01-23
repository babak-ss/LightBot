using LightBot.Core;
using UnityEngine;

namespace LightBot.LevelEditor
{
    public class LevelEditorInputController : MonoBehaviour
    {
        [SerializeField] private Vector3EventSO _clickEvent;
        [SerializeField] private VoidEventSO _initializeMapEvent;
        [SerializeField] private VoidEventSO _refreshGridMapViewEvent;
        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _clickEvent.RaiseEvent(Input.mousePosition);
            }
        }

        public void OnInitializeMapButtonClicked()
        {
            _initializeMapEvent.Raise();
        }

        public void OnViewMapButtonClicked()
        {
            _refreshGridMapViewEvent.Raise();
        }
    }
}
