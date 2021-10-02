using UnityEngine;
using System.Collections.Generic;
using Fumiko.Systems.Execution;

namespace Fumiko.Procedural
{
    public class ProceduralLimbGenerator : MonoBehaviour
    {
        public ProceduralLimbLibrary library;
        public Transform ProceduralBoneGeneratorPrefab;

        public ProceduralLimb[] limbs;

        bool initialized;

        public List<GameObject> spawnedGenerators;

        void Update()
        {
            if (!library)
            {
                return;
            }

            if (!initialized)
            {
                if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.INITIALIZATION))
                {
                    return;
                }

                spawnedGenerators = new List<GameObject>();

                for (int currentLimb = 0; currentLimb < limbs.Length; currentLimb++)
                {
                    ProceduralLimb limb = limbs[currentLimb];

                    for (int limbInstance = 0; limbInstance < limb.count; limbInstance++)
                    {
                        GameObject generator = GameObject.Instantiate(ProceduralBoneGeneratorPrefab).gameObject;
                        generator.name = "RandomLimb_" + currentLimb + "_" + limbInstance;
                        generator.transform.parent = this.gameObject.transform;
                        generator.transform.localPosition = Vector3.zero + limb.offset;

                        ProceduralBoneGenerator generatorProperties = generator.GetComponent<ProceduralBoneGenerator>();

                        generatorProperties.choiceOfBones = new Transform[limb.bonesFromLibrary.Length];

                        for (int boneFromLibrary = 0; boneFromLibrary < generatorProperties.choiceOfBones.Length; boneFromLibrary++)
                        {
                            ProceduralLimbLibrary.ProceduralLimbLibraryEntry entry = library.limbs.Find(x => x.id == limb.bonesFromLibrary[boneFromLibrary]);

                            if (entry != null)
                            {
                                generatorProperties.choiceOfBones[boneFromLibrary] = entry.prefab;
                            }
                        }

                        generatorProperties.minSplitSize = limb.minSplitSize;
                        generatorProperties.maxSplitSize = limb.maxSplitSize;
                        generatorProperties.choiceOfSplits = new Transform[limb.splitsFromLibrary.Length];

                        for (int splitFromLibrary = 0; splitFromLibrary < generatorProperties.choiceOfSplits.Length; splitFromLibrary++)
                        {
                            ProceduralLimbLibrary.ProceduralLimbLibrarySplitEntry entry = library.splits.Find(x => x.id == limb.splitsFromLibrary[splitFromLibrary]);

                            if (entry != null)
                            {
                                generatorProperties.choiceOfSplits[splitFromLibrary] = entry.prefab;
                            }
                        }

                        generatorProperties.type = limb.type;
                        generatorProperties.algorithmDirectionChanges = limb.algorithmDirectionChanges;
                        generatorProperties.algorithmType = limb.algorithmType;
                        generatorProperties.minNumberOfBones = limb.minLength;
                        generatorProperties.maxNumberOfBones = limb.maxLength;
                        generatorProperties.chanceForSplit = limb.chanceForSplit;

                        spawnedGenerators.Add(generator);
                    }
                }

                for (int i = 0; i < spawnedGenerators.Count; i++)
                {
                    spawnedGenerators[i].SetActive(true);
                }

                initialized = true;
            }
        }
    }
}