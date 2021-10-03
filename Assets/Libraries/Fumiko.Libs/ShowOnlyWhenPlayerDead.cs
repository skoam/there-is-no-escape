using UnityEngine;
using UnityEngine.UI;
using Fumiko.Systems.GameSystem;

public class ShowOnlyWhenPlayerDead : MonoBehaviour
{
    public Image imageRenderer;
    public float maximumAlpha;
    public float minimumAlpha;
    public float speedIn = 1;
    public float speedOut = 6;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (imageRenderer == null)
        {
            imageRenderer = GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (!GameSystem.instance.CallCustomQuery(CommonQueryNames.IsPlayerAlive).IsTrue)
        {
            if (spriteRenderer != null)
            {
                if (spriteRenderer.color.a < maximumAlpha)
                {
                    spriteRenderer.color += new Color(0, 0, 0, speedIn * Time.deltaTime);
                }
            }
            if (imageRenderer != null)
            {
                if (imageRenderer.color.a < maximumAlpha)
                {
                    imageRenderer.color += new Color(0, 0, 0, speedIn * Time.deltaTime);
                }
            }
        }
        else
        {
            if (spriteRenderer != null)
            {
                if (spriteRenderer.color.a > minimumAlpha)
                {
                    spriteRenderer.color -= new Color(0, 0, 0, speedOut * Time.deltaTime);
                }
            }
            if (imageRenderer != null)
            {
                if (imageRenderer.color.a > minimumAlpha)
                {
                    imageRenderer.color -= new Color(0, 0, 0, speedOut * Time.deltaTime);
                }
            }
        }
    }
}