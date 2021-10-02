using System.Collections.Generic;
using UnityEngine;

namespace Fumiko.Common
{
    [CreateAssetMenu(fileName = "DefaultMaterialLibrary", menuName = "Fumiko.Common/MaterialLibrary", order = 1)]
    public class MaterialLibrary : ScriptableObject
    {
        [System.Serializable]
        public class MaterialMap
        {
            public string originalName = "";
            public Material material;
        }

        [System.Serializable]
        public class MaterialCombination
        {
            public string id = "";
            public MaterialMap[] maps;
        }

        [System.Serializable]
        public class MaterialCollection
        {
            public string id = "";
            public List<MaterialCombination> combinations;
        }

        public List<MaterialCollection> collections;
    }
}