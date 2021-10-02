using System.Collections.Generic;
using UnityEngine;

namespace Fumiko.Procedural
{
    [CreateAssetMenu(fileName = "ProceduralLimbLibrary", menuName = "Fumiko.Procedural/ProceduralLimbLibrary", order = 1)]
    public class ProceduralLimbLibrary : ScriptableObject
    {
        [System.Serializable]
        public class ProceduralLimbLibraryEntry
        {
            public int id = 0;
            public Transform prefab;
        }

        [System.Serializable]
        public class ProceduralLimbLibrarySplitEntry
        {
            public int id = 0;
            public Transform prefab;
        }

        public List<ProceduralLimbLibraryEntry> limbs;
        public List<ProceduralLimbLibrarySplitEntry> splits;
    }
}