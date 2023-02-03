using UnityEngine;

namespace LightBot.Core
{
    [CreateAssetMenu(menuName = "Events/Void Event")]
    public class VoidEventSO : EventBaseSO
    {
        public delegate void OnEventRaiseHandler();
        private event OnEventRaiseHandler OnEventRaised;

        public void Subscribe(OnEventRaiseHandler listener)
        {
            OnEventRaised += listener;
        }

        public void Unsubscribe(OnEventRaiseHandler listener)
        {
            OnEventRaised -= listener;
        }

        public override void Raise()
        {
            // Debug.Log($"Event '{this.name}' Raised!");
            OnEventRaised?.Invoke();
        }
    }
}
