using UnityEngine;
using Fumiko.Common;

namespace Fumiko.Procedural
{
    public class ProceduralBoneGenerator : MonoBehaviour
    {
        public Transform[] choiceOfBones;
        public Transform[] choiceOfSplits;
        public ProceduralBone[] spawnedBones;

        public ProceduralLimbType type;
        public ProceduralBoneAlgorithmType algorithmType;

        public int algorithmDirectionChanges = 2;

        [Header("Properties")]
        public int maxNumberOfBones = 10;
        public int minNumberOfBones = 5;
        public Transform defaultPrefab;
        public int chanceOfLeg = 50;
        public Transform legPrefab;
        public int chanceOfArm = 50;
        public Transform armPrefab;
        public int chanceForSplit = 0;
        public int minSplitSize = 0;
        public int maxSplitSize = 0;

        [Range(0, 100)]
        public int reachPercentage = 100;

        [Range(0, 100)]
        public int initialExpandPercentage = 99;

        public float gapSize = 0.1f;

        public Transform target;
        public string findTargetByTag;

        int numberOfBones = 5;
        float expand = 0;
        GameObject generatedTarget;
        GameObject generatedPoint;
        GameObject generatedSplit;
        ProceduralPoint pointProperties;
        BasicMovementGenerator movementGenerator;
        float fullLength = 0;
        float calculatedLength = 0;

        public void Start()
        {
            numberOfBones = Random.Range(minNumberOfBones, maxNumberOfBones);

            spawnedBones = new ProceduralBone[numberOfBones];
            ProceduralBone previousBoneInChain = null;

            GameObject generatedRoot = new GameObject("ProceduralPoint_Root");
            generatedRoot.transform.parent = this.transform;
            generatedRoot.transform.localPosition = Vector3.zero;
            generatedRoot.AddComponent(typeof(ProceduralPoint));

            pointProperties = generatedRoot.GetComponent<ProceduralPoint>();
            pointProperties.type = ProceduralPointType.END;

            for (int i = 0; i < numberOfBones; i++)
            {
                GameObject generatedBone = new GameObject("ProceduralBone_" + i);

                generatedBone.SetActive(false);
                generatedBone.transform.parent = this.transform;

                generatedBone.transform.localPosition = Vector3.zero;

                generatedBone.AddComponent(typeof(ProceduralBone));

                ProceduralBone boneProperties = generatedBone.GetComponent<ProceduralBone>();

                boneProperties.algorithmType = algorithmType;
                boneProperties.algorithmDirectionChanges = algorithmDirectionChanges;

                boneProperties.boneObject = choiceOfBones[Random.Range(0, choiceOfBones.Length)];
                boneProperties.previousBoneInChain = previousBoneInChain;

                if (previousBoneInChain)
                {
                    previousBoneInChain.nextBoneInChain = boneProperties;
                }

                spawnedBones[i] = boneProperties;
                previousBoneInChain = boneProperties;

                generatedBone.SetActive(true);
                spawnedBones[i].Spawn();
            }

            for (int i = 0; i < spawnedBones.Length; i++)
            {
                fullLength += spawnedBones[i].length + gapSize;
            }

            calculatedLength = fullLength * ((float)reachPercentage / 100);

            if (type == ProceduralLimbType.NONE)
            {
                if (Random.Range(0, 100) < chanceOfLeg)
                {
                    type = ProceduralLimbType.LEG;
                }

                if (Random.Range(0, 100) < chanceOfArm)
                {
                    type = ProceduralLimbType.ARM;
                }
            }

            if (type == ProceduralLimbType.ARM && armPrefab)
            {
                generatedTarget = GameObject.Instantiate(armPrefab).gameObject;
            }
            else if (type == ProceduralLimbType.LEG && legPrefab)
            {
                generatedTarget = GameObject.Instantiate(legPrefab).gameObject;
            }
            else
            {
                if (defaultPrefab)
                {
                    generatedTarget = GameObject.Instantiate(defaultPrefab).gameObject;
                }
                else
                {
                    generatedTarget = new GameObject("ProceduralPoint_Target");
                    generatedTarget.AddComponent(typeof(BasicMovementGenerator));
                }
            }

            movementGenerator = generatedTarget.GetComponent<BasicMovementGenerator>();

            generatedTarget.AddComponent(typeof(ProceduralPoint));
            pointProperties = generatedTarget.GetComponent<ProceduralPoint>();
            pointProperties.type = ProceduralPointType.START;

            generatedTarget.transform.parent = this.transform;

            expand = calculatedLength * ((float)initialExpandPercentage / 100);
            generatedTarget.transform.localPosition = new Vector3(Random.Range(-expand, expand), Random.Range(-expand, expand), 0);
            generatedTarget.transform.localPosition = Vector3.ClampMagnitude(generatedTarget.transform.localPosition, calculatedLength);

            switch (type)
            {
                case ProceduralLimbType.LEG:
                    movementGenerator.findGroundFromParentPosition = true;
                    movementGenerator.findNearestGround = true;
                    movementGenerator.findGroundInRadius = calculatedLength;
                    break;
                case ProceduralLimbType.ARM:
                    movementGenerator.followTarget = target;

                    if (findTargetByTag != null && findTargetByTag != "")
                    {
                        movementGenerator.followTag = findTargetByTag;
                    }

                    break;
                default:
                    if (!defaultPrefab)
                    {
                        movementGenerator.sinusY = true;
                        movementGenerator.cosinusX = true;
                        movementGenerator.circularMovementSpeed = 0.2f;
                        movementGenerator.circularMovementSize = 0.5f;
                    }
                    break;
            }

            movementGenerator.stayAtParent = this.transform;
            movementGenerator.maxDistanceToParent = calculatedLength;

            if (Random.Range(0, 100) < chanceForSplit)
            {
                int numberOfSplits = Random.Range(minSplitSize, maxSplitSize + 1);

                for (int i = 0; i < numberOfSplits; i++)
                {
                    Transform split = GameObject.Instantiate(choiceOfSplits[Random.Range(0, choiceOfSplits.Length)]);
                    split.transform.parent = generatedTarget.transform;
                    split.transform.localPosition = Vector3.zero;

                    split.gameObject.SetActive(true);
                }
            }

            for (int i = 0; i < spawnedBones.Length; i++)
            {
                if (i == 0)
                {
                    spawnedBones[i].attachPoint = generatedRoot.transform;
                    spawnedBones[i].targetPoint = spawnedBones[i + 1].startPoint;
                }
                else if (i < spawnedBones.Length - 1)
                {
                    spawnedBones[i].attachPoint = spawnedBones[i - 1].endPoint;
                    spawnedBones[i].targetPoint = spawnedBones[i + 1].startPoint;
                }
                else
                {
                    spawnedBones[i].attachPoint = spawnedBones[i - 1].endPoint;
                    spawnedBones[i].targetPoint = generatedTarget.transform;
                }

                spawnedBones[i].lastBoneInChain = spawnedBones[spawnedBones.Length - 1];
                spawnedBones[i].boneFamily = spawnedBones;
                spawnedBones[i].fullLength = fullLength;
                spawnedBones[i].positionInChain = i;
                spawnedBones[i].firstBoneInChain = spawnedBones[0];
            }
        }

        public ProceduralLimbType Type
        {
            get
            {
                return type;
            }
        }
    }
}