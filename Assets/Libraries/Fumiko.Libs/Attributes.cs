using UnityEngine;

[System.Serializable]
public class Attributes : MonoBehaviour
{
    [Header("Manual Values")]
    public float gravity = 9.81f;
    public float minGravity = 2;
    public float gravityReactionSpeed = 5;
    public int maxHealth = 3;
    public int maxIllness = 3;
    public float maxSpeed = 4.5f;
    public float minSpeed = 0.1f;
    public float minSpeedClearWaitTime = 0.2f;
    public float maxVelocityX = 20;
    public float maxVelocityYDownGrounded = -2f;
    public float maxVelocityYDown = -8f;
    public float maxVelocityYUp = 8f;
    public int jumpInAirCount = 2;
    public float speedIncreasePerSecond = 8f;
    public float invincibilitySecondsOnHit = 2;

    [Header("Runtime Values")]
    public float currentGravity = 0;
    public int health = 3;
    public int illness = 0;
    public float invincible = 0;
    public int currentJumps = 0;
    public float timeSinceLastJump = 0;
    public float currentRunningTime = 0;
    public float currentSpeed;
    public float currentSpeedClearWaitTime = 0;
    public float forwardMomentumIntensity = 0;
    public float verticalMomentumIntensity = 0;
    public float walkDelay;
    public bool leftGroundFlag = false;
    public bool grounded = true;
    public Vector2 velocity = Vector2.zero;
    public Vector2 targetVelocity = Vector2.zero;
    public AnimationCurve forwardMomentum;
    public AnimationCurve verticalMomentum;
    public bool slash;
    public float slashDuration;
    public float attackResetTime;
    public Collider2D hitbox;
    public float lastAttackPress;

    public float AverageSpeed
    {
        get
        {
            return (maxSpeed + minSpeed) / 2;
        }
    }

    private void Update()
    {
        CalculateTimers();
    }

    private void Start()
    {
        health = maxHealth;
    }

    void CalculateTimers()
    {
        if (lastAttackPress >= 0)
        {
            lastAttackPress -= Time.deltaTime;
        }

        if (invincible > 0)
        {
            invincible -= Time.deltaTime;
        }

        if (walkDelay > 0)
        {
            walkDelay -= Time.deltaTime;
        }
        else
        {
            walkDelay = 0;
        }
    }

    public bool IsAlive
    {
        get
        {
            return health > 0;
        }
    }
}