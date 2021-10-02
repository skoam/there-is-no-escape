using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Fumiko.Systems.Execution;

namespace Fumiko.Common
{
    public class IsProjectile : MonoBehaviour
    {
        [Info("How much the lerp function lerps per frame")]
        public float movementPerFrame = 1;

        private Vector3 lastRequestedDirection = Vector3.zero;

        public bool disabled;

        public float damp = 1;
        public bool disableZMovement = true;

        private Vector3 movePosition = Vector3.zero;
        private Vector3 moveVector = Vector3.zero;
        private Vector3 origin = Vector3.zero;

        private float checkExecutionStep = 0.5f;
        private float currentExecutionCheckTime = 0;

        public float minMoveDistance = 0.05f;

        public Vector3 forward = Vector3.right;

        [HideInInspector]
        public bool initialized;

        public class Force
        {
            public Force(Vector3 inDirection, float inForce)
            {
                direction = inDirection;
                force = inForce;
            }

            public Vector3 direction;
            public float force;

            public bool remove;
        }

        public List<Force> forces;

        [ContextMenu("Push Random")]
        public void PushRandom()
        {
            Push(
                new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    0
                ),
                Random.Range(0.1f, 1.5f)
            );
        }

        public void Push(Vector3 direction, float force = 1)
        {
            direction = direction.normalized;
            lastRequestedDirection = direction;

            if (forces.Count > 10)
            {
                forces.RemoveAt(0);
            }

            forces.Add(
                new Force(direction, force)
            );
        }

        void UpdateMovement()
        {
            origin = this.transform.position;
            moveVector = Vector3.zero;

            for (int i = 0; i < forces.Count; i++)
            {
                moveVector += forces[i].direction * forces[i].force;
                forces[i].force -= Time.fixedDeltaTime * damp;

                if (forces[i].force <= 0)
                {
                    forces[i].remove = true;
                }
            }

            if (forces.Count > 0)
            {
                forces.RemoveAll(x => x.remove == true);
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

        void CheckExecution()
        {
            currentExecutionCheckTime += Time.deltaTime;

            if (currentExecutionCheckTime > checkExecutionStep)
            {
                currentExecutionCheckTime = 0;
            }
            else
            {
                return;
            }

            if (ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.MOVEMENT))
            {
                disabled = false;
            }
            else
            {
                disabled = true;
            }
        }

        void FixedUpdate()
        {
            if (disabled)
            {
                return;
            }

            UpdateMovement();
        }

        void Update()
        {
            if (!initialized)
            {
                if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.INITIALIZATION))
                {
                    return;
                }

                forces = new List<Force>();
                initialized = true;
            }

            CheckExecution();

            if (disabled)
            {
                return;
            }

            for (int i = 0; i < forces.Count; i++)
            {
                UnityEngine.Debug.DrawRay(
                    this.transform.position, forces[i].direction * forces[i].force,
                    Color.yellow);
            }

            if (movePosition != this.transform.position && movePosition != Vector3.zero)
            {
                MoveToPosition();
            }
        }

        void MoveToPosition()
        {
            this.transform.position = Vector3.Lerp(
                this.transform.position,
                movePosition,
                Time.deltaTime * movementPerFrame
            );
        }
    }
}