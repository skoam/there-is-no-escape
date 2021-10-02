using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotLocLerp : MonoBehaviour
{
    public Transform target;
    public float positionLerpFactor = 1;
    public float rotationLerpFactor = 1;

    public bool disablePositionLerp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!disablePositionLerp)
        {
            this.transform.position = Vector3.Lerp(
                this.transform.position,
                target.transform.position,
                Time.deltaTime * positionLerpFactor
            );
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                target.transform.position, Time.deltaTime * positionLerpFactor
            );
        }

        this.transform.rotation = Quaternion.Lerp(
            this.transform.rotation,
            target.transform.rotation,
            Time.deltaTime * rotationLerpFactor
        );
    }
}
