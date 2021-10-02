using System.Collections.Generic;
using UnityEngine;

namespace Fumiko.Common
{
    [CreateAssetMenu(fileName = "DefaultPositionLibrary", menuName = "Fumiko.Common/PositionLibrary", order = 1)]
    public class EnforcedPositions : ScriptableObject
    {
        [System.Serializable]
        public class EnforcedPosition
        {
            public string id = "";
            public Vector3 position;
        }

        public List<EnforcedPosition> positions;
    }
}