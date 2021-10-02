using UnityEngine;

namespace Fumiko.Common
{
    public class rotateMeSlow : MonoBehaviour
    {
        public float rotationSpeed = 6.0f;

        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed / transform.localScale.x * 2 * Time.deltaTime));
        }
    }
}