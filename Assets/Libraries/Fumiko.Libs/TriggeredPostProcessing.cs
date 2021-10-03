using UnityEngine;
using UnityEngine.Rendering;

using Fumiko.Systems.Execution;

[RequireComponent(typeof(Volume))]
public class TriggeredPostProcessing : MonoBehaviour
{
    public Attributes trackAttributes;
    public string findAttributesByTag;

    public bool activatedByDeath = false;
    public bool activatedByInvincibility = false;

    public float activationBlendingSpeed = 1;
    public bool active = true;
    public float activeValue = 1;

    private Volume volume;

    public void activate()
    {
        active = true;
    }

    public void checkForActivationTriggers()
    {
        bool activation = false;

        if (ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.CHARACTERS))
        {
            if (activatedByInvincibility)
            {
                if (trackAttributes.invincible > 0)
                {
                    activation = true;
                }
            }

            if (activatedByDeath)
            {
                if (!trackAttributes.IsAlive)
                {
                    activation = true;
                }
            }
        }

        if (activation)
        {
            activate();
        }
        else
        {
            deactivate();
        }
    }

    public void deactivate()
    {
        active = false;
    }

    private void Start()
    {
        volume = GetComponent<Volume>();
    }

    private void GetAttributes()
    {
        if (findAttributesByTag != "")
        {
            GameObject AttributesContainer = GameObject.FindGameObjectWithTag(findAttributesByTag);
            trackAttributes = AttributesContainer.GetComponent<Attributes>();
        }
    }

    float SearchDelay = 0.5f;
    float currentSearchDelay = 0;
    private void Update()
    {
        if (!trackAttributes)
        {
            if (currentSearchDelay == 0)
            {
                currentSearchDelay = SearchDelay;
                GetAttributes();
            }
            else
            {
                currentSearchDelay -= Time.deltaTime;
            }

            return;
        }

        checkForActivationTriggers();

        if (active)
        {
            if (volume.weight < activeValue)
            {
                volume.weight += Time.deltaTime * activationBlendingSpeed;
            }
            else
            {
                volume.weight = activeValue;
            }
            return;
        }
        else
        {
            if (volume.weight > 0)
            {
                volume.weight -= Time.deltaTime * activationBlendingSpeed;
            }
            else
            {
                volume.weight = 0;
            }
            return;
        }
    }
}