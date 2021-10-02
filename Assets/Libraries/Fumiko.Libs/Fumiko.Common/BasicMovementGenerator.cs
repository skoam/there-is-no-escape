using UnityEngine;

using Fumiko.Systems.Debug;

namespace Fumiko.Common
{
    public class BasicMovementGenerator : MonoBehaviour
    {
        public float movementPerFrame = 1;
        public Vector3 constantMovementDirection;
        public Vector3 randomMovementDirection;
        public Vector3 constantRotation;

        public float constantMovementSpeed;

        public bool randomizeMovement;
        public float randomMovementSpeedMin;
        public float randomMovementSpeedMax;
        public float randomizeMovementUpdateFrequency;

        public float circularMovementSpeed = 1;
        public float circularMovementSize = 1;

        public bool disableZMovement = true;

        public bool sinusX;
        public bool sinusY;

        public bool cosinusX;
        public bool cosinusY;

        private Vector3 circle = Vector3.zero;

        private float currentCirclePosition = 0;

        public float minMoveDistance = 0.05f;

        public Transform followTarget;
        public string followTag;
        public float followSpeedMin = 0.1f;
        public float followSpeedMax = 2;
        public float followUpdateFrequency = 1;
        public float followRestDistance = 0.5f;

        Vector3 lastKnownFollowPosition = Vector3.zero;
        float currentFollowUpdateFrequency = 0;
        float followSpeed = 0.1f;

        public Transform stayAtParent;
        public float maxDistanceToParent = 5;
        public float parentStrength = 2;
        public float returnToParentChance = 0;
        public float returnToParentDuration = 0.5f;

        float randomMovementSpeed;
        bool returnToParent = false;
        float newDistanceToTarget = 0;
        float currentReturnToParentDuration = 0;

        public bool findNearestGround;
        public bool findGroundFromParentPosition;
        public float groundMinParentDistanceForUpdate = 0;
        public float groundSearchInterval = 2f;
        public int groundLayer = 8;
        public float findGroundInRadius = 7;
        public float groundStrength = 2;

        float currentGroundSearchInterval = 0;
        float currentRandomizeMovementTimer = 0;

        Vector3 currentGroundTarget = Vector3.zero;
        Vector3 nearestGroundPosition = Vector3.zero;
        Vector3 lastValidGround = Vector3.zero;
        Vector3 groundSearchOrigin = Vector3.zero;
        Vector3 fromSelfToMovePosition = Vector3.zero;
        Vector3 movePosition = Vector3.zero;
        Vector3 moveVector = Vector3.zero;
        Vector3 origin = Vector3.zero;

        private void FixedUpdate()
        {
            if (followTag != null && followTag != "" && followTarget == null)
            {
                GameObject target = GameObject.FindGameObjectWithTag(followTag);

                if (target)
                {
                    followTarget = target.transform;
                }
            }

            float time = Time.fixedDeltaTime;

            if (currentCirclePosition < 360)
            {
                currentCirclePosition += circularMovementSpeed * time;
            }
            else
            {
                currentCirclePosition = 0;
            }

            circle = Vector3.zero;

            if (sinusX)
            {
                circle.x += Mathf.Sin(currentCirclePosition);
            }

            if (sinusY)
            {
                circle.y += Mathf.Sin(currentCirclePosition);
            }

            if (cosinusX)
            {
                circle.x += Mathf.Cos(currentCirclePosition);
            }

            if (cosinusY)
            {
                circle.y += Mathf.Cos(currentCirclePosition);
            }

            circle *= circularMovementSize;

            moveVector = Vector3.zero;

            origin = this.transform.position;

            moveVector += constantMovementDirection * constantMovementSpeed;
            moveVector += circle;

            if (randomizeMovement)
            {
                currentRandomizeMovementTimer += time;

                if (currentRandomizeMovementTimer >= randomizeMovementUpdateFrequency)
                {
                    randomMovementDirection = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2));
                    randomMovementSpeed = Random.Range(randomMovementSpeedMin, randomMovementSpeedMax);
                    currentRandomizeMovementTimer = 0;
                }

                moveVector += randomMovementDirection * randomMovementSpeed;
            }

            if (followTarget)
            {
                currentFollowUpdateFrequency += time;

                if (currentFollowUpdateFrequency > followUpdateFrequency)
                {
                    lastKnownFollowPosition = followTarget.transform.position;
                    followSpeed = Random.Range(followSpeedMin, followSpeedMax);
                    currentFollowUpdateFrequency = 0;
                }

                if (lastKnownFollowPosition != Vector3.zero)
                {
                    if (Vector3.Distance(origin, lastKnownFollowPosition) > followRestDistance)
                    {
                        moveVector += Vector3.ClampMagnitude(lastKnownFollowPosition - origin, followSpeed);
                    }
                }
            }

            if (findNearestGround)
            {
                currentGroundSearchInterval += time;

                nearestGroundPosition = origin;
                groundSearchOrigin = origin;

                if (findGroundFromParentPosition)
                {
                    groundSearchOrigin = stayAtParent.transform.position;
                }

                if (currentGroundSearchInterval > groundSearchInterval &&
                    (lastValidGround == Vector3.zero ||
                    groundMinParentDistanceForUpdate == 0 ||
                    Vector3.Distance(origin, stayAtParent.transform.position) > groundMinParentDistanceForUpdate))
                {
                    Collider2D[] groundHits = Physics2D.OverlapCircleAll(groundSearchOrigin, findGroundInRadius, (1 << groundLayer));

                    float nearestGroundDistance = findGroundInRadius;

                    for (int i = 0; i < groundHits.Length; i++)
                    {
                        Vector3 closestPoint = Physics2D.ClosestPoint(groundSearchOrigin, groundHits[i]);
                        float distanceToPoint = Vector3.Distance(groundSearchOrigin, closestPoint);

                        if (distanceToPoint < nearestGroundDistance)
                        {
                            nearestGroundDistance = distanceToPoint;
                            nearestGroundPosition = closestPoint;
                        }
                    }

                    currentGroundSearchInterval = 0;
                }

                // Debug.DrawRay(this.transform.position, lastValidGround - this.transform.position, Color.red, 0.1f);

                if (nearestGroundPosition == origin)
                {
                    if (lastValidGround != Vector3.zero)
                    {
                        currentGroundTarget = lastValidGround;
                    }
                }
                else
                {
                    currentGroundTarget = nearestGroundPosition;
                    lastValidGround = nearestGroundPosition;
                }

                if (stayAtParent && Vector3.Distance(stayAtParent.transform.position, currentGroundTarget) > maxDistanceToParent)
                {
                    currentGroundTarget = stayAtParent.transform.position;
                    lastValidGround = Vector3.zero;
                }

                // Debug.DrawRay(this.transform.position, currentGroundTarget - this.transform.position, Color.red, 0.1f);

                moveVector += Vector3.ClampMagnitude(currentGroundTarget - origin, groundStrength);
            }

            if (returnToParentChance > 0)
            {
                if (stayAtParent && !returnToParent && Random.Range(0, 1000) < returnToParentChance)
                {
                    returnToParent = true;
                    currentReturnToParentDuration = 0;
                }
            }

            if (currentReturnToParentDuration > returnToParentDuration)
            {
                returnToParent = false;
                currentReturnToParentDuration = 0;
            }

            if (stayAtParent && returnToParent && currentReturnToParentDuration < returnToParentDuration)
            {
                moveVector = Vector3.ClampMagnitude(stayAtParent.transform.position - origin, parentStrength);
                currentReturnToParentDuration += time;
            }

            if (disableZMovement)
            {
                moveVector.z = 0;
            }

            if (moveVector.magnitude < minMoveDistance)
            {
                moveVector = Vector3.zero;
            }

            // Debug.DrawRay(this.transform.position, moveVector, Color.blue, 0.1f);

            if (moveVector != Vector3.zero)
            {
                movePosition = origin + moveVector;
            }

            if (constantRotation.magnitude <= 0)
            {
                return;
            }

            Vector3 rotationVector = transform.localRotation.eulerAngles;

            rotationVector += constantRotation * time;

            transform.localRotation = transform.localRotation * Quaternion.Euler(constantRotation * time);
        }

        void Update()
        {
            if (movePosition != this.transform.position && movePosition != Vector3.zero)
            {

                if (stayAtParent)
                {
                    newDistanceToTarget = Vector3.Distance(movePosition, stayAtParent.transform.position);

                    if (newDistanceToTarget > maxDistanceToParent)
                    {
                        movePosition = stayAtParent.transform.position + (movePosition - origin).normalized * maxDistanceToParent;

                        if (followTarget)
                        {
                            movePosition = origin + Vector3.ClampMagnitude(movePosition - origin, followSpeed);
                        }
                    }
                }

                if (followTarget && Vector3.Distance(movePosition, followTarget.transform.position) < followRestDistance)
                {
                    fromSelfToMovePosition = movePosition - origin;
                    movePosition = origin + Vector3.ClampMagnitude(fromSelfToMovePosition, fromSelfToMovePosition.magnitude - followRestDistance);
                }

                MoveToPosition();
            }
        }

        void MoveToPosition()
        {
            // Debug.DrawRay(this.transform.position, movePosition - origin, Color.blue);
            this.transform.position = Vector3.Lerp(this.transform.position, movePosition, Time.deltaTime * movementPerFrame);
        }
    }
}