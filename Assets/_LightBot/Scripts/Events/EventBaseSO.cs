using UnityEngine;

namespace LightBot.Events
{
    public abstract class EventBaseSO :ScriptableObject
    {
        public abstract void Raise();
    }
    public abstract class EventBaseSO<T> :ScriptableObject
    {
        public abstract void Raise(T t);
    }
}