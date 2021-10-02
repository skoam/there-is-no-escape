using UnityEngine;

namespace Fumiko.Common
{
    [System.Serializable]
    public abstract class BasicMovementModule : MonoBehaviour
    {
        public abstract Vector3 Calculate(Transform origin);
    }
}
