using UnityEngine;

namespace Fumiko.Common
{
    public class RandomSizeAndRotation : MonoBehaviour
    {
        public float minRandomSize = 1;
        public float maxRandomSize = 10;

        public float maxRandomRotation = 180;

        public bool rotateX = true;
        public bool rotateY = true;
        public bool rotateZ = true;

        // Start is called before the first frame update
        void Start()
        {
            this.transform.Rotate(new Vector3(
                Random.Range(-maxRandomRotation, maxRandomRotation),
                Random.Range(-maxRandomRotation, maxRandomRotation),
                rotateZ ? Random.Range(-maxRandomRotation, maxRandomRotation) : 0
            ), Space.World);

            float randomSize = Random.Range(minRandomSize, maxRandomSize);

            this.transform.localScale = new Vector3(
                randomSize,
                randomSize,
                randomSize
            );
        }
    }
}