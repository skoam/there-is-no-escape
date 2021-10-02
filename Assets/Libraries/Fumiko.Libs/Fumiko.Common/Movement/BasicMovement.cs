using UnityEngine;
using System.Collections.Generic;

using Fumiko.Systems.Execution;

namespace Fumiko.Common
{
    public class BasicMovement : MonoBehaviour
    {
        public bool debug;
        public bool disabled = true;
        public bool disableZMovement = true;
        public float movementPerFrame = 1;
        public float minMoveDistance = 0.05f;

        public BasicMovementModule[] movements;

        private bool initialized;
        private Vector3 movePosition = Vector3.zero;
        private Vector3 moveVector = Vector3.zero;
        private Vector3 origin = Vector3.zero;

        void Update()
        {
            if (disabled)
            {
                return;
            }

            if (!initialized)
            {
                if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.INITIALIZATION))
                {
                    return;
                }

                movements = this.GetComponents<BasicMovementModule>();

                if (movements.Length > 0)
                {
                    initialized = true;
                }

                return;
            }

            if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.MOVEMENT))
            {
                return;
            }

            if (movePosition != this.transform.position && movePosition != Vector3.zero)
            {
                MoveToPosition();
            }
        }

        void FixedUpdate()
        {
            if (disabled)
            {
                return;
            }

            if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.MOVEMENT))
            {
                return;
            }

            float time = Time.fixedDeltaTime;

            origin = this.transform.position;
            moveVector = Vector3.zero;

            for (int i = 0; i < movements.Length; i++)
            {
                moveVector += movements[i].Calculate(this.transform);
            }

            if (disableZMovement)
            {
                moveVector.z = 0;
            }

            if (moveVector.magnitude < minMoveDistance)
            {
                moveVector = Vector3.zero;
            }

            movePosition = origin + moveVector;
        }

        void MoveToPosition()
        {
            if (debug)
            {
                Debug.DrawRay(this.transform.position, movePosition - origin, Color.blue);
            }

            this.transform.position = Vector3.Lerp(
                this.transform.position,
                movePosition,
                Time.deltaTime * movementPerFrame
            );
        }
    }
}