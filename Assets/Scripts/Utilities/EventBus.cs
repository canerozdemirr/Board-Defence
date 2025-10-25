using System;
using System.Collections.Generic;
using Events.Interfaces;

namespace Utilities
{
    public static class EventBus
    {
        private interface IEventDispatcher { }
        
        private class EventDispatcher<TEvent> : IEventDispatcher where TEvent : IEvent
        {
            private readonly List<Action<TEvent>> _listenerList = new(8);
            public void AddListener(Action<TEvent> listener)
            {
                if (!_listenerList.Contains(listener))
                {
                    _listenerList.Add(listener);
                }
            }
            
            public void RemoveListener(Action<TEvent> listener)
            {
                _listenerList.Remove(listener);
            }
            
            public void Dispatch(TEvent eventData)
            {
                List<Action<TEvent>> listenersCopy = new(_listenerList);
                
                foreach (Action<TEvent> listener in listenersCopy)
                {
                    listener(eventData);
                }
            }
            
            public bool HasListeners => _listenerList.Count > 0;
        }
        
        private static readonly Dictionary<Type, IEventDispatcher> _dispatcherMap = new();

        
        public static void Subscribe<TEvent>(Action<TEvent> listener) where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);
            
            if (!_dispatcherMap.TryGetValue(eventType, out IEventDispatcher dispatcher))
            {
                dispatcher = new EventDispatcher<TEvent>();
                _dispatcherMap[eventType] = dispatcher;
            }
            
            ((EventDispatcher<TEvent>)dispatcher).AddListener(listener);
        }

        public static void Unsubscribe<TEvent>(Action<TEvent> listener) where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);

            if (!_dispatcherMap.TryGetValue(eventType, out IEventDispatcher dispatcher))
                return;
            
            ((EventDispatcher<TEvent>)dispatcher).RemoveListener(listener);
                
            if (!((EventDispatcher<TEvent>)dispatcher).HasListeners)
            {
                _dispatcherMap.Remove(eventType);
            }
        }
        
        public static void Publish<TEvent>(TEvent eventData) where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);
            
            if (_dispatcherMap.TryGetValue(eventType, out IEventDispatcher dispatcher))
            {
                ((EventDispatcher<TEvent>)dispatcher).Dispatch(eventData);
            }
        }
        
        public static bool HasSubscribers<TEvent>() where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);
            
            return _dispatcherMap.TryGetValue(eventType, out IEventDispatcher dispatcher) && ((EventDispatcher<TEvent>)dispatcher).HasListeners;
        }
        
        public static void ClearAll()
        {
            _dispatcherMap.Clear();
        }
    }
}