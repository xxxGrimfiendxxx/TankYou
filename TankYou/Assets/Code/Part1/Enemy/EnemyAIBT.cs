using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBT : MonoBehaviour
{
    public Transform player; // Reference to the player GameObject
    public Transform flag; // Reference to the flag GameObject
    public Transform baseLocation; // Base location for depositing the flag
    public float attackRange = 15f; // Range for attacking the player
    public float movementSpeed = 5f; // Speed of movement
    public float turnSpeed = 90f; // Speed of turning (degrees per second)
    public GameObject prefabToSpawn; // Reference to the prefab to spawn for shooting
    public float spawnDistance = 2f; // Distance in front of the enemy to spawn the prefab
    public float shootCooldown = 2f; // Cooldown for shooting action
    public float stateChangeCooldown = 1f; // Cooldown for changing states
    public bool isHeldCheck;
    public int readEnemyScore;
    public static bool isEnemyHit = false;
    public int maxHealth = 100; // Maximum health of the enemy
    public int currentHealth; // Current health of the enemy
    public Transform respawnPoint; // Respawn point for the enemy
    public enum State
    {
        Retrieve,
        Shoot,
        Deposit
    }

    private State currentState;
    private float lastStateChangeTime;
    private float lastShootTime;

    void Start()
    {
        currentState = State.Retrieve;
        lastStateChangeTime = -stateChangeCooldown; // Allow immediate state changes
    }

    void Update()
    {
        isHeldCheck = FlagController.isHeld;
        readEnemyScore = FlagController.EnemyScore;
        // Check if enough time has passed to change state
        if (Time.time - lastStateChangeTime >= stateChangeCooldown)
        {
            // Execute behavior tree based on current state
            switch (currentState)
            {
                case State.Retrieve:
                    ExecuteRetrieve();
                    break;
                case State.Shoot:
                    ExecuteShoot();
                    break;
                case State.Deposit:
                    ExecuteDeposit();
                    break;
            }

            // Update the last state change time
            lastStateChangeTime = Time.time;
        }
    }

    // Behavior tree node for shoot state
    void ExecuteShoot()
    {
        Debug.Log("Shoot state");

        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // Shoot at the player
            Shoot();
        }

        // Transition to Retrieve state if flag is not held
        if (isHeldCheck == false)
        {
            currentState = State.Retrieve;
        }
    }

    // Behavior tree node for retrieve state
    void ExecuteRetrieve()
    {
        Debug.Log("Retrieve state");

        // Move towards the flag
        MoveTowardsFlag();

        // Transition to Shoot state if player is within attack range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = State.Shoot;
        }

        // Transition to Deposit state if flag is held
        if (isHeldCheck == true)
        {
            currentState = State.Deposit;
        }
    }

    // Behavior tree node for deposit state
    void ExecuteDeposit()
    {
        Debug.Log("Deposit state");

        // Move towards the base location
        MoveTowardsBaseLocation();

        // Transition to Shoot state if player is within attack range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = State.Shoot;
        }

        // Transition to Retrieve state if flag is not held
        if (isHeldCheck == false)
        {
            currentState = State.Retrieve;
        }
    }

    void Shoot()
    {
        Debug.Log("Shooting");
        // Check cooldown
        if (Time.time - lastShootTime >= shootCooldown)
        {
            // Look towards the player
            transform.LookAt(player.position);

            // Calculate spawn position in front of the enemy
            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;

            // Spawn the prefab at the calculated position with the same rotation as the enemy
            Instantiate(prefabToSpawn, spawnPosition, transform.rotation);

            // Update last shoot time
            lastShootTime = Time.time;
        }
    }

    void MoveTowardsFlag()
    {
        Debug.Log("Moving towards flag");
        // Calculate direction towards the flag
        Vector3 direction = (flag.position - transform.position).normalized;

        // Rotate towards the flag
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        transform.Rotate(Vector3.up, Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), turnSpeed * Time.deltaTime));

        // Move towards the flag
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    void MoveTowardsBaseLocation()
    {
        Debug.Log("Moving towards base location");
        // Calculate direction towards the base location
        Vector3 direction = (baseLocation.position - transform.position).normalized;

        // Rotate towards the base location
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        transform.Rotate(Vector3.up, Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), turnSpeed * Time.deltaTime));

        // Move towards the base location
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // Assuming the tag of the bullet object is "Bullet"
        {


            BulletHit(40); // Enemy loses 40 health when hit by a bullet

            isEnemyHit = true; // Set isEnemyHit to true upon collision with bullet
            StartCoroutine(ResetHitStatus()); // Start coroutine to reset isEnemyHit after 1 second
        }
    }
    IEnumerator ResetHitStatus()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        isEnemyHit = false; // Reset isEnemyHit after 1 second
    }
    public void BulletHit(int damage)
    {
        currentHealth -= damage; // Reduce current health by the amount of damage
        if (currentHealth <= 0)
        {
            Die(); // If health drops to zero or below, call the Die() method
        }
    }
    // Method to handle enemy death
    void Die()
    {
        // Reset health to maximum
        currentHealth = maxHealth;
        // Respawn the enemy at the specified respawn point
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
    }
}
