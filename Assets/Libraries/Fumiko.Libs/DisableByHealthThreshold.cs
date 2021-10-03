using UnityEngine;
using UnityEngine.UI;
using Fumiko.Systems.Execution;
using Fumiko.Systems.GameSystem;

public class DisableByHealthThreshold : MonoBehaviour
{
    public int threshold;
    public bool findPlayerAttributes;
    public Attributes fromAttributes;
    private Image image;

    private void Start()
    {
        if (findPlayerAttributes)
        {
            fromAttributes = GameObject.FindGameObjectWithTag("Player").GetComponent<Attributes>();
        }

        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (!ExecutionQuerySystem.instance.ExecutionAllowed(ExecutionType.MOVEMENT))
        {
            image.enabled = false;
            return;
        }

        if (fromAttributes.health < threshold)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }
}