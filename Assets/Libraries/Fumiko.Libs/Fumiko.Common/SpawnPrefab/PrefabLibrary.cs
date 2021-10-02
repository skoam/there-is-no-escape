using System.Collections.Generic;
using UnityEngine;

namespace Fumiko.Common
{
    [CreateAssetMenu(fileName = "DefaultPrefabLibrary", menuName = "Fumiko.Common/PrefabLibrary", order = 1)]
    public class PrefabLibrary : ScriptableObject
    {
        [System.Serializable]
        public class PrefabCollection
        {
            public string id = "";

            public List<PrefabEntry> variants;
        }

        [System.Serializable]
        public class PrefabEntry
        {
            public string id = "";
            public Transform prefab;
        }

        public List<PrefabCollection> collections;
    }
}