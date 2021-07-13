using Enterprise.Blocksvalley.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enterprise.Blocksvalley.Observer
{
    public class ObserverController : Singleton<ObserverController>
    {
        public Dictionary<object, Dictionary<Action<ObserverParam>, BaseBehavior>> observList = new Dictionary<object, Dictionary<Action<ObserverParam>, BaseBehavior>>();

        public void AddListener(object key, BaseBehavior obj, Action<ObserverParam> callback)
        {
            if (!observList.ContainsKey(key))
            {
                var actions = new Dictionary<Action<ObserverParam>, BaseBehavior>();
                actions.Add(callback, obj);
                observList.Add(key, actions);
            }
            else
            {
                observList[key].Add(callback, obj);
            }
        }

        public void SendMessageToSubscriber(object key, object data = null, int target = -1)
        {
            if (observList.ContainsKey(key))
            {
                var observerParam = new ObserverParam
                {
                    data = data,
                    key = key,
                    target = target,
                };
                var actions = observList[key];

                if (target == -1)
                {
                    for(int i = 0; i < actions.Count; i++)
                    {
                        var tempBehavior = actions.Values.ElementAt(i);
                        tempBehavior.OnHandleMessage(observerParam, actions.Keys.ElementAt(i));
                    }
                } 
                else
                {
                    for (int i = 0; i < actions.Count; i++)
                    {
                        var tempBehavior = actions.Values.ElementAt(i);
                        var tempBehaviorID = tempBehavior.gameObject.GetInstanceID();

                        if (tempBehaviorID == target)
                        {
                            tempBehavior.OnHandleMessage(observerParam, actions.Keys.ElementAt(i));
                        }
                    }
                }
            }
        }

        public void RemoveListener(object key, BaseBehavior obj, Action<ObserverParam> callback)
        {
            if(observList.ContainsKey(key))
            {
                var actions = observList[key];
                for(int i = 0; i < actions.Count; i++)
                {
                    if(actions.Keys.ElementAt(i) == callback && actions.Values.ElementAt(i) == obj)
                    {
                        actions.Remove(callback);
                    }
                }
            }
        }

        public void RemoveAllListeners(BaseBehavior obj)
        {
            foreach(var item in observList)
            {
                var actions = item.Value;
                if (actions.ContainsValue(obj))
                {
                    for(int i = 0; i < actions.Count; i++)
                    {
                        if(actions.Values.ElementAt(i) == obj)
                        {
                            actions.Remove(actions.Keys.ElementAt(i));
                        }
                    }
                }
            }
        }
    }
}
