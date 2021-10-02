using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Fumiko.Common
{
    [ExecuteAlways]
    public class ApplyMaterialsFromLibrary : MonoBehaviour
    {
        [Header("ID must be specified.")]
        public MaterialLibrary materialLibrary;

        public string id = "";
        public SelectStyle style;

        [Header("Optional")]
        public string combinationId;

        [ContextMenu("Apply")]
        void Apply()
        {
            CheckLibrary();

            if (!materialLibrary)
            {
                return;
            }

            Renderer renderer = GetComponent<Renderer>();

            MaterialLibrary.MaterialCollection collection = materialLibrary.collections.Find(x => x.id == id);

            if (collection != null)
            {
                if (style == SelectStyle.FIRST)
                {
                    ApplyMaterialCombination(renderer, collection.combinations.First());
                }

                if (style == SelectStyle.LAST)
                {
                    ApplyMaterialCombination(renderer, collection.combinations.Last());
                }

                if (style == SelectStyle.RANDOM || style == SelectStyle.ALL)
                {
                    ApplyMaterialCombination(renderer, collection.combinations[Random.Range(0, collection.combinations.Count)]);
                }

                if (style == SelectStyle.SPECIFIC)
                {
                    ApplyMaterialCombination(renderer, collection.combinations.Find(x => x.id == combinationId));
                }
            }
        }

        void ApplyMaterialCombination(Renderer renderer, MaterialLibrary.MaterialCombination combination)
        {
            Material[] sharedMaterials = renderer.sharedMaterials;

            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                for (int u = 0; u < combination.maps.Length; u++)
                {
                    if (renderer.sharedMaterials[i].name.Contains(combination.maps[u].originalName))
                    {
                        sharedMaterials[i] = combination.maps[u].material;
                        Debug.Log("Applying " + combination.maps[u].material.name + " to " + renderer.sharedMaterials[i].name + " in " + this.name);
                    }
                }
            }

            renderer.sharedMaterials = sharedMaterials;
        }

        void Start()
        {
            Apply();
        }

        void OnEnable()
        {
            Apply();
        }

        void CheckLibrary()
        {
            if (!materialLibrary)
            {
                string[] libraries = AssetDatabase.FindAssets("DefaultMaterialLibrary");

                if (libraries.Length > 0)
                {
                    string atPath = AssetDatabase.GUIDToAssetPath(libraries[0]);
                    materialLibrary = (MaterialLibrary)AssetDatabase.LoadAssetAtPath(atPath, typeof(MaterialLibrary));
                }

                return;
            }
        }
    }
}