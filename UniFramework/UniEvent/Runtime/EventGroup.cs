﻿using System;
using System.Collections.Generic;

namespace UniFramework.Event
{
    public class EventGroup
    {
        private readonly Dictionary<System.Type, List<Action<IEventMessage>>> _cachedListener = new Dictionary<System.Type, List<Action<IEventMessage>>>();
        private int? _groupId = null;

        public EventGroup() { }

        public EventGroup(int groupId)
        {
            _groupId = groupId;
        }

        /// <summary>
        /// 添加一个监听
        /// </summary>
        public void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
        {
            System.Type eventType = typeof(TEvent);
            if (_cachedListener.ContainsKey(eventType) == false)
                _cachedListener.Add(eventType, new List<Action<IEventMessage>>());

            if (_cachedListener[eventType].Contains(listener) == false)
            {
                _cachedListener[eventType].Add(listener);
                if (_groupId != null)
                    UniEvent.AddListener((int)_groupId, listener);
                else
                    UniEvent.AddListener(eventType, listener);
            }
            else
            {
                UniLogger.Warning($"Event listener is exist : {eventType}");
            }
        }

        /// <summary>
        /// 移除所有缓存的监听
        /// </summary>
        public void RemoveAllListener()
        {
            foreach (var pair in _cachedListener)
            {
                System.Type eventType = pair.Key;
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (_groupId != null)
                        UniEvent.RemoveListener((int)_groupId, pair.Value[i]);
                    else
                        UniEvent.RemoveListener(eventType, pair.Value[i]);
                }
                pair.Value.Clear();
            }
            _cachedListener.Clear();
        }
    }
}