using Enterprise.Blocksvalley.Observer;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Enterprise.Blocksvalley.Gameplay
{
    public class CubeBehavior : BaseBehavior
    {
        protected ObserverController observer;
        public Animation anim;
        private int id;

        private Vector3 pos;

        private void Awake()
        {
            observer = ObserverController.Instance;
            id = gameObject.GetInstanceID();
        }

        private void Start()
        {
            Initialized();
        }

        protected void Initialized()
        {
            observer.AddListener("RECEIVE_COMMAND", this, ReceiveCommandCallback);

            pos = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y), Mathf.Floor(transform.position.z));
            transform.position = pos;

            var _data = new KeyValuePair<int, string>(id, pos.ToString());
            observer.SendMessageToSubscriber("ADD_TO_CELL", _data);
        }

        protected void startAnimation(Vector3 curPos, Vector3 nextPos)
        {
            Direction upOrDown = Direction.None;
            if (nextPos[1] < curPos[1])
            {
                upOrDown = Direction.Down;
            }
            else
            {
                upOrDown = Direction.Up;
            }

            var _data = new KeyValuePair<int, string>(id, nextPos.ToString());
            pos = nextPos;
            observer.SendMessageToSubscriber("ADD_TO_CELL", _data);
            AnimationClip clip = CustomAnimation.setAnimationTo(gameObject, nextPos, upOrDown);

            anim.AddClip(clip, clip.name);
            anim.Play(clip.name);
        }

        private Vector3? CheckPos(CubeController controllerRef, Vector3 nextPos, int increment = -2)
        {
            var newPos = nextPos;
            newPos.y += increment;
            var check = controllerRef.checkPosition(newPos.ToString());
            if (!check && newPos.y >= 0)
            {
                return newPos;
            } 
            else if(increment >= 1)
            {
                return null;
            }
            return CheckPos(controllerRef, nextPos, increment + 1);
        }

        private Vector3? GetNewPos(Vector3 _pos, Direction direction)
        {
            switch (direction)
            {
                case Direction.Front:
                    return new Vector3(_pos.x, _pos.y, _pos.z + 1);
                case Direction.Back:
                    return new Vector3(_pos.x, _pos.y, _pos.z - 1);
                case Direction.Left:
                    return new Vector3(_pos.x - 1, _pos.y, _pos.z);
                case Direction.Right:
                    return new Vector3(_pos.x + 1, _pos.y, _pos.z);
                default:
                    return null;
            }
        }

        private void ReceiveCommandCallback(ObserverParam param)
        {
            var _param = param.data as CubeCommandParam;

            var cellAboveThisCube = new Vector3(pos.x, pos.y + 1, pos.z);
            var checkAbove = _param.controller.checkPosition(cellAboveThisCube.ToString());
            
            if (checkAbove) return;

            var newPos = GetNewPos(pos, _param.direction);
            
            if (newPos == null) return;
            
            var nextPosAvailable = CheckPos(_param.controller, (Vector3) newPos);

            if (nextPosAvailable == null) return;

            var diff = (Vector3) nextPosAvailable - pos;
            Debug.Log("diff: " + diff);
            if (Math.Abs(diff.y) >= 2) return;

            startAnimation(pos, (Vector3) nextPosAvailable);
        }

        public virtual void OnAnimationEnded()
        {
            observer.SendMessageToSubscriber("UNLOCK_QUEUE");
        }
    }
}