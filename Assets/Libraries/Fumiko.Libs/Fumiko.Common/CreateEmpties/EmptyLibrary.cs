using System.Collections.Generic;
using UnityEngine;

namespace Fumiko.Common
{
    [CreateAssetMenu(fileName = "DefaultEmptyLibrary", menuName = "Fumiko.Common/EmptyLibrary", order = 1)]
    public class EmptyLibrary : ScriptableObject
    {
        [System.Serializable]
        public class EmptyList
        {
            public string id = "";
            public string[] empties;

            [Header("Components can be referenced by creating an empty prefab.")]
            public MonoBehaviour[] components;
        }

        public List<EmptyList> emptyLists;
    }
}