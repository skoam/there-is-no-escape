using elZach.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fumiko.Systems.Input;

namespace InfectiousVoid
{
    public class PeriodicSpawner : MonoBehaviour
    {
        public enum SpawnSamples { Instantiate, Spawn, SpawnAndStart, SpawnAndReset }

        public GameObject[] prefab;
        Component prefabBehaviour;
        public SpawnSamples spawnType = SpawnSamples.Spawn;
        public bool spawnOnKeyPress;
        public InputCases key;
        public float spawnTime = 1f;
        public bool spawnTimeIsRandom = false;
        public float radius = 1f;
        public bool spawnOnHorizontalLine = false;
        public bool spawnInZ = false;
        public int spawnCount = 1;
        public int maximumSpawns = 200;

        public float despawnTime = 4f;
        List<GameObject> spawned = new List<GameObject>();
        List<float> spawnedTime = new List<float>();

        private float currentTime = 0;
        private float nextRandomSpawnTime = 0;
        private float t = 0;

        void Start()
        {
        }

        private void Update()
        {
            t = Time.time;
            currentTime += Time.deltaTime;

            if (nextRandomSpawnTime == 0)
            {
                nextRandomSpawnTime = Random.Range(0, spawnTime);
            }

            if (spawnOnKeyPress && !InputMapSystem.instance.getButtonHold(key))
            {
                currentTime = spawnTime;
            }

            if (currentTime > (spawnTimeIsRandom ? nextRandomSpawnTime : spawnTime))
            {
                if (spawnOnKeyPress && InputMapSystem.instance.getButtonHold(key))
                {
                    Spawn();
                }
                else if (!spawnOnKeyPress)
                {
                    Spawn();
                }

                currentTime = 0;
                nextRandomSpawnTime = 0;
            }

            if (despawnTime > 0)
            {
                for (int i = spawned.Count - 1; i >= 0; i--)
                {
                    if (t - spawnedTime[i] >= despawnTime)
                    {
                        if (spawnType == SpawnSamples.Instantiate)
                        {
                            Destroy(spawned[i]);
                        }
                        else
                        {
                            spawned[i].Despawn();
                        }

                        spawned.RemoveAt(i);
                        spawnedTime.RemoveAt(i);
                    }
                }
            }
        }

        void Spawn()
        {
            if (spawned.Count < maximumSpawns)
            {
                int startAt = 0;

                for (int i = startAt; i < spawnCount; i++)
                {
                    int randomPrefabIndex = Random.Range(0, prefab.Length);

                    GameObject clone;

                    switch (spawnType)
                    {
                        case SpawnSamples.Instantiate:
                            clone = Instantiate(prefab[randomPrefabIndex]);
                            break;
                        case SpawnSamples.Spawn:
                            clone = prefab[randomPrefabIndex].Spawn();
                            break;
                        case SpawnSamples.SpawnAndStart:
                            clone = prefab[randomPrefabIndex].SpawnAndStart();
                            break;
                        case SpawnSamples.SpawnAndReset:
                            prefabBehaviour = (Component)prefab[randomPrefabIndex].GetComponent<ICallStart>();
                            clone = prefab[randomPrefabIndex].SpawnAndReset(null, prefabBehaviour);
                            break;
                        default:
                            clone = Instantiate(prefab[randomPrefabIndex]);
                            break;
                    }

                    Vector2 randomCircle = Random.insideUnitCircle * radius;

                    Vector3 randomCircleIn3D =
                        new Vector3(
                            randomCircle.x,
                            randomCircle.y,
                            Random.Range(-radius, radius)
                        );

                    if (!spawnInZ)
                    {
                        randomCircleIn3D.z = 0;
                    }

                    if (spawnOnHorizontalLine)
                    {
                        randomCircleIn3D.y = 0;
                    }

                    clone.transform.position = transform.position + randomCircleIn3D;

                    spawned.Add(clone);
                    spawnedTime.Add(Time.time);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
