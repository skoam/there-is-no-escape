using UnityEngine;

using Fumiko.Common;

namespace Fumiko.Procedural
{
    public class ProceduralBone : MonoBehaviour
    {
        public Transform boneObject;
        public ProceduralBoneAlgorithmType algorithmType;

        public ProceduralBone previousBoneInChain;
        public ProceduralBone nextBoneInChain;
        public ProceduralBone firstBoneInChain;
        public ProceduralBone lastBoneInChain;

        public ProceduralPoint[] chainPoints;

        public ProceduralBone[] boneFamily;

        public Transform targetPoint;
        public Transform attachPoint;

        public Transform startPoint;
        public Transform endPoint;

        float LerpMultiplier = 20.0f;
        float LerpMultiplierMovement = 40.0f;
        float LerpMultiplierRotation = 20.0f;
        float LerpMultiplierMultiplier = 1;
        public bool algorithmMirrorMode = true;

        public float length = 0;
        public int positionInChain = 0;

        Vector3 fromRootToTarget;
        Vector3 fromTargetToAttach;
        Vector3 toTarget;
        Vector3 toAttach;

        Vector3 rotationToApply = Vector3.zero;
        Vector3 positionToApply = Vector3.zero;

        float distanceToTarget = 0;

        public float fullLength = 0;
        int numberOfBones = 0;

        public bool initialized;

        float reduction;
        int reductionCenter;

        float reductionToFullLengthRatio;

        Vector3 lastPosition;

        float intendedLength;

        public int algorithmDirectionChanges = 2;
        float bonePointOffset;
        int directionChangeEvery;
        int direction;
        float stepSize;
        Vector3 directionT;
        Vector3 lastDirectionT;
        Vector3 rootToTargetDirection;

        bool lerpMode = false;
        int mirrorDirection = 1;
        float waveMirrorLerp = 0;
        public float waveMirrorLerpTime = 1;
        bool waveMirrorLerpActive = false;

        public Direction rotationAxis = Direction.RIGHT;
        public Vector3 rotationOffsetAxis = Vector3.up;
        public float rotationOffset = 0;
        public bool autoSpawn = false;
        Vector3 finalRotationVector = Vector3.zero;

        void Start()
        {
            if (autoSpawn)
            {
                Spawn();
            }
        }

        [ContextMenu("Spawn and Initialize")]
        public void Spawn()
        {
            if (boneObject)
            {
                Transform generatedBone = GameObject.Instantiate(boneObject, this.transform.position, Quaternion.identity, this.transform);
                chainPoints = generatedBone.gameObject.GetComponentsInChildren<ProceduralPoint>();
            }

            CollectStartAndEndPoints();
            CalculateLength();

            if (algorithmType == ProceduralBoneAlgorithmType.SNAP_TARGET)
            {
                lerpMode = true;
            }

            waveMirrorLerp = -waveMirrorLerpTime;

            positionToApply = this.transform.position;

            initialized = true;
        }

        void Update()
        {
            if (!initialized)
            {
                return;
            }

            MoveToPosition();
            ApplyRotation();
        }

        void ApplyRotation()
        {
            rotationToApply = Quaternion.AngleAxis(rotationOffset, rotationOffsetAxis) * rotationToApply;

            if (lerpMode)
            {
                LerpMultiplier = LerpMultiplierRotation * LerpMultiplierMultiplier;

                finalRotationVector = Vector3.Lerp(this.transform.right, rotationToApply,
                    Time.deltaTime * LerpMultiplier);
            }
            else
            {
                finalRotationVector = rotationToApply;
            }

            switch (rotationAxis)
            {
                case Direction.FORWARD:
                    this.transform.forward = finalRotationVector;
                    break;
                case Direction.BACKWARD:
                    this.transform.forward = -finalRotationVector;
                    break;
                case Direction.RIGHT:
                    this.transform.right = finalRotationVector;
                    break;
                case Direction.LEFT:
                    this.transform.right = -finalRotationVector;
                    break;
                case Direction.UP:
                    this.transform.up = finalRotationVector;
                    break;
                case Direction.DOWN:
                    this.transform.up = -finalRotationVector;
                    break;
                default:
                    this.transform.right = finalRotationVector;
                    break;
            }
        }

        void MoveToPosition()
        {
            if (lerpMode)
            {
                LerpMultiplier = LerpMultiplierMovement * LerpMultiplierMultiplier;

                this.transform.position = Vector3.Lerp(this.transform.position, positionToApply,
                  Time.deltaTime * LerpMultiplier * Vector3.Distance(this.transform.position, positionToApply));
                return;
            }

            this.transform.position = positionToApply;
        }

        void FixedUpdate()
        {
            if (!initialized)
            {
                return;
            }

            if (numberOfBones == 0)
            {
                numberOfBones = boneFamily.Length;
            }

            CalculatePositionsAndRotations();
        }

        void CalculatePositionsAndRotations()
        {
            if (this == lastBoneInChain)
            {
                if (algorithmType == ProceduralBoneAlgorithmType.WAVE)
                {
                    MoveBonesToCurve();
                }

                if (algorithmType == ProceduralBoneAlgorithmType.SNAP_TARGET)
                {
                    for (int i = boneFamily.Length - 1; i > 0; i--)
                    {
                        boneFamily[i].MoveToTarget();
                    }

                    for (int i = 0; i < boneFamily.Length; i++)
                    {
                        boneFamily[i].CorrectToAttach();
                    }
                }
            }

            if (this == firstBoneInChain)
            {
                Attach();
            }

            RotateToTarget();
        }

        public void RotateToTarget()
        {
            rotationToApply = (Vector2)(startPoint.position - targetPoint.position);
        }

        public void MoveBonesToCurve()
        {
            fromRootToTarget = lastBoneInChain.targetPoint.position - firstBoneInChain.startPoint.position;
            rootToTargetDirection = fromRootToTarget.normalized;

            distanceToTarget = fromRootToTarget.magnitude;
            reduction = (fullLength - distanceToTarget);
            reductionCenter = numberOfBones / 2;
            reductionToFullLengthRatio = 1 - (reduction / fullLength);

            if (waveMirrorLerpActive)
            {
                if (waveMirrorLerp > -waveMirrorLerpTime)
                {
                    waveMirrorLerp -= Time.fixedDeltaTime;
                }
                else
                {
                    waveMirrorLerp = -waveMirrorLerpTime;
                    waveMirrorLerpActive = false;
                }
            }

            if (reduction < 0)
            {
                reduction = 0;
            }

            lastPosition = firstBoneInChain.startPoint.position;

            intendedLength = fullLength - reduction;

            bonePointOffset = 0;
            direction = 1;

            algorithmDirectionChanges = Mathf.Clamp(algorithmDirectionChanges, 1, numberOfBones - 1);
            directionChangeEvery = numberOfBones / (algorithmDirectionChanges + 1);

            stepSize = intendedLength / numberOfBones;

            if (lastDirectionT != Vector3.zero)
            {
                directionT = lastDirectionT;
            }

            if (algorithmMirrorMode)
            {
                if (lastBoneInChain.targetPoint.position.x < firstBoneInChain.startPoint.position.x)
                {
                    if (mirrorDirection == 1)
                    {
                        directionT = Quaternion.AngleAxis(90, Vector3.forward) * rootToTargetDirection;
                    }

                    if (mirrorDirection == -1 && !waveMirrorLerpActive)
                    {
                        waveMirrorLerpActive = true;
                        waveMirrorLerp = waveMirrorLerpTime;
                    }

                    if (waveMirrorLerpActive && waveMirrorLerp <= 0)
                    {
                        mirrorDirection = 1;
                    }
                }
                else
                {
                    if (mirrorDirection == -1)
                    {
                        directionT = Quaternion.AngleAxis(-90, Vector3.forward) * rootToTargetDirection;
                    }

                    if (mirrorDirection == 1 && !waveMirrorLerpActive)
                    {
                        waveMirrorLerpActive = true;
                        waveMirrorLerp = waveMirrorLerpTime;
                    }

                    if (waveMirrorLerpActive && waveMirrorLerp <= 0)
                    {
                        mirrorDirection = -1;
                    }
                }
            }
            else
            {
                directionT = Quaternion.AngleAxis(90, Vector3.forward) * rootToTargetDirection;
            }

            lastDirectionT = directionT;

            for (int i = 1; i < numberOfBones; i++)
            {
                stepSize = intendedLength / (int)(fullLength / boneFamily[i - 1].length);

                bonePointOffset = Mathf.Sqrt(Mathf.Pow(boneFamily[i - 1].length, 2) - Mathf.Pow(stepSize, 2));

                if (float.IsNaN(bonePointOffset))
                {
                    bonePointOffset = 0;
                }

                if (waveMirrorLerp > -waveMirrorLerpTime)
                {
                    bonePointOffset *= Mathf.Abs(waveMirrorLerp);
                }

                if (float.IsNaN(stepSize))
                {
                    stepSize = boneFamily[i - 1].length;
                }

                boneFamily[i].positionToApply =
                    lastPosition +
                    rootToTargetDirection.normalized * stepSize +
                    directionT.normalized * bonePointOffset * direction;

                // Debug.DrawRay(lastPosition, lastPosition - boneFamily[i].positionToApply, Color.green);

                lastPosition = boneFamily[i].positionToApply;

                if ((i + 1) % directionChangeEvery == 0)
                {
                    direction *= -1;
                }
            }
        }

        public void Attach()
        {
            positionToApply = attachPoint.position;
        }

        public void MoveToTarget()
        {
            fromTargetToAttach = attachPoint.position - targetPoint.transform.position;
            toTarget = fromTargetToAttach.normalized * length;
            positionToApply = targetPoint.position + toTarget;
        }

        public void CorrectToAttach()
        {
            toAttach = attachPoint.position - positionToApply;

            if (toAttach.magnitude > 0.05f)
            {
                positionToApply += toAttach * 0.2f;
            }
        }

        [ContextMenu("Calculate Length")]
        public void CalculateLength()
        {
            length = Vector2.Distance(startPoint.transform.position, endPoint.transform.position);
        }

        [ContextMenu("Calculate Full Length")]
        public void CalculateFullLength()
        {
            fullLength = 0;
            for (int i = 0; i < boneFamily.Length; i++)
            {
                fullLength += boneFamily[i].length;
            }
        }

        [ContextMenu("Collect Start and End Points")]
        public void CollectStartAndEndPoints()
        {
            for (int i = 0; i < chainPoints.Length; i++)
            {
                if (chainPoints[i].type == ProceduralPointType.START)
                {
                    startPoint = chainPoints[i].transform;
                }

                if (chainPoints[i].type == ProceduralPointType.END)
                {
                    endPoint = chainPoints[i].transform;
                }
            }
        }

        [ContextMenu("Create Empty Start and End Points")]
        public void CreateEmptyStartAndEndPoints()
        {
            GameObject emptyStartPoint = new GameObject();
            emptyStartPoint.name = "StartPoint";
            emptyStartPoint.transform.parent = this.gameObject.transform;
            emptyStartPoint.transform.localPosition = Vector3.zero;
            emptyStartPoint.transform.localRotation = Quaternion.identity;
            emptyStartPoint.transform.localScale = Vector3.one;
            emptyStartPoint.AddComponent<ProceduralPoint>();
            emptyStartPoint.GetComponent<ProceduralPoint>().type = ProceduralPointType.START;

            GameObject emptyEndPoint = new GameObject();
            emptyEndPoint.name = "EndPoint";
            emptyEndPoint.transform.parent = this.gameObject.transform;
            emptyEndPoint.transform.localPosition = Vector3.zero;
            emptyEndPoint.transform.localRotation = Quaternion.identity;
            emptyEndPoint.transform.localScale = Vector3.one;
            emptyEndPoint.AddComponent<ProceduralPoint>();
            emptyEndPoint.GetComponent<ProceduralPoint>().type = ProceduralPointType.END;

            chainPoints = new ProceduralPoint[2];
            chainPoints[0] = emptyStartPoint.GetComponent<ProceduralPoint>();
            chainPoints[1] = emptyEndPoint.GetComponent<ProceduralPoint>();
        }

        [ContextMenu("Find Attach and Target")]
        public void FindAttachAndTarget()
        {
            for (int i = 1; i < boneFamily.Length - 1; i++)
            {
                if (boneFamily[i].gameObject == this.gameObject)
                {
                    attachPoint = boneFamily[i - 1].endPoint;
                    targetPoint = boneFamily[i + 1].startPoint;
                }
            }
        }

        [ContextMenu("Find Position in Chain")]
        public void FindPositionInChain()
        {
            for (int i = 0; i < boneFamily.Length; i++)
            {
                if (boneFamily[i].gameObject == this.gameObject)
                {
                    positionInChain = i;
                }

                if (i < boneFamily.Length - 1)
                {
                    boneFamily[i].nextBoneInChain = boneFamily[i + 1];
                }

                if (i > 0)
                {
                    boneFamily[i].previousBoneInChain = boneFamily[i - 1];
                }

                boneFamily[i].firstBoneInChain = boneFamily[0];
                boneFamily[i].lastBoneInChain = boneFamily[boneFamily.Length - 1];
            }
        }
    }
}