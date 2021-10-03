using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;

using Fumiko.Systems.GameSystem;
using Fumiko.Systems.Execution;

// [RequireComponent(typeof(PostProcessVolume))]
public class LocalAreaPostProcessing : MonoBehaviour
{
    public float activationBlendingSpeed = 1;
    public float activationDistance = 5;
    public float activationFullBlendDistance = 0;

    public bool active = true;
    public Transform trigger;
    public bool triggeredByHero = false;

    // private PostProcessVolume volume;

    public void activate()
    {
        active = true;
    }

    public void deactivate()
    {
        active = false;
    }

    private void Start()
    {
        // volume = GetComponent<PostProcessVolume>();

        if (triggeredByHero)
        {
            trigger = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.CHARACTERS))
        {
            return;
        }

        if (!active)
        {
            /*if (volume.weight > 0)
            {
                volume.weight -= Time.deltaTime * activationBlendingSpeed;
            }

            if (volume.weight < 0)
            {
                volume.weight = 0;
            }*/

            return;
        }

        if (!triggeredByHero)
        {
            return;
        }

        if (!trigger)
        {
            trigger = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }

        float distance = Vector2.Distance(trigger.transform.position, this.transform.position);
        if (distance < activationDistance)
        {
            float insideDistance = (activationDistance - distance);

            /*if (activationFullBlendDistance > 0)
            {
                volume.weight = insideDistance / activationFullBlendDistance;
            }
            else
            {
                volume.weight = 1;
            }*/
        }
        else
        {
            // volume.weight = 0;
        }
    }
}