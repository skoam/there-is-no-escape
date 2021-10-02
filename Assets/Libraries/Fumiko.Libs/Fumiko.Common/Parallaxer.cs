using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fumiko.Common
{

    public class Parallaxer : MonoBehaviour
    {
        public float maxDepth = 50f;
        public Vector2 strength = Vector2.right;

        public Transform[] paralaxingObjects;
        //Vector2[] startPositions;
        Transform target;

        // Start is called before the first frame update
        void Start()
        {
            target = Camera.main.transform;
            /*startPositions = new Vector2[paralaxingObjects.Length];
            for(int i=0; i< paralaxingObjects.Length; i++)
            {
                startPositions[i] = paralaxingObjects[i].position;
            }*/
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 camOffset = target.position;
            foreach (var trans in paralaxingObjects)
            {
                float factor = trans.position.z / maxDepth;
                trans.position = new Vector3(camOffset.x * factor * strength.x, camOffset.y * factor * strength.y, trans.position.z);
            }
        }
    }
}