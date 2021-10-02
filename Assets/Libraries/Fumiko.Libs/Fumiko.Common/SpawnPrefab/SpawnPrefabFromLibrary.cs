using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Fumiko.Common
{
    [ExecuteAlways]
    public class SpawnPrefabFromLibrary : MonoBehaviour
    {
        [Header("ID must be specified.")]
        public PrefabLibrary prefabLibrary;

        public string id = "";
        public SelectStyle style;

        [Header("Optional")]
        public string prefabId = "";
        public bool spawnOnceAndKeep;

        private bool spawned;

        private List<Transform> prefabCache = new List<Transform>();

        [ContextMenu("Spawn Prefabs")]
        public void Spawn()
        {
            if (spawned && spawnOnceAndKeep)
            {
                return;
            }

            CheckLibrary();

            if (!prefabLibrary)
            {
                return;
            }

            prefabCache.Clear();

            try
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform spawnedPrefab = transform.GetChild(i);

                    if (spawnedPrefab.name.Contains("_spawned_prefab_"))
                    {
                        prefabCache.Add(spawnedPrefab);
                    }
                }

#if UNITY_EDITOR
                prefabCache.ForEach(x => DestroyImmediate(x.gameObject));
#else
                prefabCache.ForEach(x => Destroy(x.gameObject));
#endif
            }
            catch
            {
                return;
            }

            PrefabLibrary.PrefabCollection collection = prefabLibrary.collections.Find(x => x.id == id);
            prefabCache.Clear();

            if (collection != null)
            {
                if (style == SelectStyle.FIRST)
                {
                    SpawnPrefab(collection.variants.First().prefab);
                }

                if (style == SelectStyle.LAST)
                {
                    SpawnPrefab(collection.variants.Last().prefab);
                }

                if (style == SelectStyle.ALL)
                {
                    for (int i = 0; i < collection.variants.Count; i++)
                    {
                        SpawnPrefab(collection.variants[i].prefab);
                    }
                }

                if (style == SelectStyle.RANDOM)
                {
                    SpawnPrefab(collection.variants[Random.Range(0, collection.variants.Count)].prefab);
                }

                if (style == SelectStyle.SPECIFIC)
                {
                    SpawnPrefab(collection.variants.Find(x => x.id == prefabId).prefab);
                }

                foreach (Transform prefab in prefabCache)
                {
                    prefab.name = "_spawned_prefab_" + prefab.name;
                    prefab.parent = this.transform;
                    prefab.localPosition = Vector3.zero;
                    prefab.localRotation = Quaternion.identity;
                    prefab.localScale = Vector3.one;
                }

                spawned = true;
            }
        }

        void SpawnPrefab(Transform prefab)
        {
            Transform spawnedPrefab = GameObject.Instantiate(prefab);
            prefabCache.Add(spawnedPrefab);

            if (!Application.isPlaying)
            {
                spawnedPrefab.gameObject.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        void Start()
        {
            Spawn();
        }

        void OnEnable()
        {
            Spawn();
        }

        void CheckLibrary()
        {
            if (!prefabLibrary)
            {
                string[] libraries = AssetDatabase.FindAssets("DefaultPrefabLibrary");

                if (libraries.Length > 0)
                {
                    string atPath = AssetDatabase.GUIDToAssetPath(libraries[0]);
                    prefabLibrary = (PrefabLibrary)AssetDatabase.LoadAssetAtPath(atPath, typeof(PrefabLibrary));
                }

                return;
            }
        }
    }
}