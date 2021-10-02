using UnityEngine;

namespace Fumiko.Common
{
    public class CopyExactLocation : MonoBehaviour
    {
        public Vector3 offset;
        public Transform target;

        private void FixedUpdate()
        {
            this.transform.position = target.transform.position + offset;
        }
    }
}
