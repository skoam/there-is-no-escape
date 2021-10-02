using UnityEngine;

namespace Fumiko.Common
{
    public class CopyLocation : MonoBehaviour
    {
        public bool accelerateByDistance;
        public float accelerationFactor = 0.01f;
        public bool forceFix;
        public float maximumX;
        public float maximumY;
        public float minActivationDistanceX = 0;
        public float minActivationDistanceY = 0;
        public float minimumDistance = 2;
        public float minimumX;
        public float minimumY;
        public bool nonFixed;
        public float offsetX;
        public float offsetY;
        public float smoothingTime = 0.5f;
        public bool smoothX;
        public bool smoothY;
        public Transform target;
        public bool X;
        public float xShakeOffset;
        public bool Y;
        private bool forceSmooth;
        private float minDistanceToStop = 1f;
        private bool movingX;
        private bool movingY;
        private Rigidbody2D myBody;
        private float timeReduction;
        private Vector3 velocity;

        private void FixedUpdate()
        {
            if (!nonFixed)
            {
                UpdateStep();
            }
        }

        private void Start()
        {
            myBody = GetComponent<Rigidbody2D>();

            myBody.MovePosition(new Vector3(
                target.transform.position.x,
                target.transform.position.y,
                this.transform.position.z));
        }

        private void Update()
        {
            if (nonFixed)
            {
                UpdateStep();
            }
        }

        private void UpdateStep()
        {
            timeReduction = 0;
            forceFix = false;
            forceSmooth = false;

            Vector3 myPos = this.transform.position;
            Vector3 newPos = new Vector3(target.position.x + offsetX, target.position.y + offsetY, this.transform.position.z);

            /*newPos.x = Mathf.Clamp(newPos.x, minimumX, maximumX);
            newPos.y = Mathf.Clamp(newPos.y, minimumY, maximumY);
            newPos.z = transform.position.z;*/

            /*if (Mathf.Abs(myPos.x - newPos.x) < minActivationDistanceX)
            {
                if (Mathf.Abs(myPos.x - newPos.x) < minDistanceToStop) {
                    movingX = false;
                }

                if (!movingX) {
                    newPos.x = myPos.x;
                }
            } else {
                movingX = true;
            }

            if (Mathf.Abs(myPos.y - newPos.y) < minActivationDistanceY)
            {
                if (Mathf.Abs(myPos.y - newPos.y) < minDistanceToStop) {
                    movingY = false;
                }

                if (!movingY) {
                    newPos.y = myPos.y;
                }
            } else {
                movingY = true;
            }

            if (accelerateByDistance && Vector3.Distance(transform.position, newPos) > minimumDistance)
            {
                timeReduction = Vector3.Distance(transform.position, newPos) / 10;
                timeReduction *= accelerationFactor;
            }

            timeReduction = Mathf.Clamp(timeReduction, 0, smoothingTime - 0.1f);

            float calculatedSmoothingTime = (float)System.Math.Round(smoothingTime - timeReduction, 2);*/

            /*if (!forceFix)
            {
                this.transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, calculatedSmoothingTime, Mathf.Infinity, Time.fixedDeltaTime);
            }*/

            /*if (X)
            {
                if (!smoothX || forceFix && !forceSmooth)
                {
                    transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);
                }
            }

            if (Y)
            {
                if (!smoothY || forceFix && !forceSmooth)
                {
                    transform.position = new Vector3(transform.position.x, newPos.y, transform.position.z);
                }
            }*/

            //myBody.MovePosition(newPos);
            myBody.position = newPos;
        }
    }
}