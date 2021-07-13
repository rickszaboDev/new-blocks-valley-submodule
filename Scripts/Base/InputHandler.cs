using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enterprise.Blocksvalley.Observer;
using Enterprise.Blocksvalley.Command;

namespace Enterprise.Blocksvalley.CustomInput
{
    public class InputHandler : MonoBehaviour
    {
        public Camera cam;
        private ObserverController observer;

        public int tolerance = 10;

        private int selectedObjectId = -1;
        private Vector3 initPosition;

        private void Awake()
        {
            observer = ObserverController.Instance;
        }

        private void Update()
        {
            ClickHandle();
        }

        private void ClickHandle()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var id = hit.collider.gameObject.GetInstanceID();
                    initPosition = Input.mousePosition;
                    selectedObjectId = id;
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                var currentMousePosition = Input.mousePosition;
                var currentDirection = CheckDirection(initPosition, currentMousePosition, tolerance);
                var commandData = new KeyValuePair<int, Direction>(selectedObjectId, currentDirection);
                observer.SendMessageToSubscriber("ENQUEUE_COMMAND", commandData);
            }
        }

        

        private Direction CheckDirection(Vector3 initPointerPos, Vector3 finalPointerPos, float tolerance)
        {
            float difX = Mathf.Abs(initPointerPos.x - finalPointerPos.x);
            float difY = Mathf.Abs(initPointerPos.y - finalPointerPos.y);

            if (difX > tolerance)
            {
                if (initPointerPos.x < finalPointerPos.x)
                    return Direction.Right;
                else if (initPointerPos.x > finalPointerPos.x)
                    return Direction.Left;
            }
            else if (difY > tolerance)
            {
                if (initPointerPos.y < finalPointerPos.y)
                    return Direction.Front;
                else if (initPointerPos.y > finalPointerPos.y)
                    return Direction.Back;
            }

            return Direction.None;
        }
    }
}