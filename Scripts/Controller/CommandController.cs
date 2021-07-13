using Enterprise.Blocksvalley.Observer;
using System.Collections.Generic;

namespace Enterprise.Blocksvalley.Command
{
    public class CommandController : BaseBehavior
    {
        public CubeController cubeController;
        private ObserverController observer;
        private Queue<KeyValuePair<int, Direction>> CommandQueue = new Queue<KeyValuePair<int, Direction>>();
        private bool blocking = false;

        private void Awake()
        {
            observer = ObserverController.Instance;
            observer.AddListener("ENQUEUE_COMMAND", this, EnqueueCommandCallback);
            observer.AddListener("UNLOCK_QUEUE", this, UnlockQueueCallback);
        }

        private void Update()
        {
            if (!blocking && CommandQueue.Count > 0)
            {
                //blocking = true;
                var nextCommand = CommandQueue.Dequeue();

                var _data = new CubeCommandParam
                {
                    direction = nextCommand.Value,
                    controller = cubeController
                };
                observer.SendMessageToSubscriber("RECEIVE_COMMAND", _data, nextCommand.Key);
            }
        }

        private void EnqueueCommandCallback(ObserverParam param)
        {
            var newCommand = (KeyValuePair<int, Direction>) param.data;
            CommandQueue.Enqueue(newCommand);
        }

        private void UnlockQueueCallback(ObserverParam param)
        {
            blocking = false;
        }
    }
}