using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfectiousVoid
{
    public class Corruption_ICallStart : MonoBehaviour, ICallStart
    {
        Rigidbody body;

        private void Awake()
        {

        }

        public void Start()
        {
            body = this.GetComponent<Rigidbody>();

            if (body)
            {
                body.AddForce(Vector3.up * 3, ForceMode.Impulse);
            }
        }

        public void Reset(ICallStart source)
        {

        }
    }
}
