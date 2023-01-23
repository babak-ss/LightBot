using UnityEngine;

namespace LightBot.Core
{
    public class GenericEventSO<T> : EventBaseSO<T>
    {
        public delegate void OnEventRaiseHandler(T t);
        private event OnEventRaiseHandler OnEventRaised;

        public void Subscribe(OnEventRaiseHandler listener)
        {
            OnEventRaised += listener;
        }

        public void Unsubscribe(OnEventRaiseHandler listener)
        {
            OnEventRaised -= listener;
        }
        
        public override void RaiseEvent(T t)
        {
            Debug.Log($"Event '{this.name}' Raised! parameter: '{t}'");
            OnEventRaised?.Invoke(t);
        }
    }
}