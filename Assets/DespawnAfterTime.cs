using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnAfterTime : MonoBehaviour
{
    public float time = 10;
    private float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > time)
        {
            this.gameObject.SetActive(false);
        }
    }
}
