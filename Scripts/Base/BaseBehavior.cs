using System;
using UnityEngine;

namespace Enterprise.Blocksvalley.Observer
{
    public class BaseBehavior : MonoBehaviour
    {
        internal void OnHandleMessage(ObserverParam param, Action<ObserverParam> value)
        {
            value(param);
        }
    }
}