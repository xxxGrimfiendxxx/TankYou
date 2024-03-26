using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Retrieve,
        Deposit
    }

    public enum Action
    {
        Move,
        Shoot
    }

    public Transform player; // Reference to the player GameObject
    public Transform flag; // Reference to the flag GameObject
    public Transform baseLocation; // Base location for depositing the flag
    public float attackRange = 15f; // Range for attacking the player
    public float movementSpeed = 5f; // Speed of movement
    public float turnSpeed = 90f; // Speed of turning (degrees per second)
    public GameObject prefabToSpawn; // Reference to the prefab to spawn for shooting
    public float spawnDistance = 3f; // Distance in front of the enemy to spawn the prefab
    public float shootCooldown = 2f; // Cooldown for shooting action
    public float stateChangeCooldown = 1f; // Cooldown for changing states
    public bool isHeldCheck;
    public int readEnemyScore;
    public static bool isEnemyHit = false;
    public int maxHealth = 100; // Maximum health of the enemy
    public int currentHealth; // Current health of the enemy
    public Transform respawnPoint; // Respawn point for the enemy

    private State currentState;
    private Action currentAction;
    private float lastShootTime;
    private float lastStateChangeTime;

    void Start()
    {
        currentState = State.Retrieve;
        lastShootTime = -shootCooldown; // Initialize last shoot time to allow immediate shooting
        lastStateChangeTime = +stateChangeCooldown; // Initialize last state change time to allow immediate state changes
        currentHealth = maxHealth; // Set current health to maximum health at the start
    }

    void Update()
    {
        isHeldCheck = FlagController1.isHeld;
        readEnemyScore = FlagController1.EnemyScore;

        // Check if enough time has passed to change state
        if (Time.time - lastStateChangeTime >= stateChangeCooldown)
        {
            // Check current state and perform corresponding actions
            switch (currentState)
            {
                case State.Retrieve:
                    if (!IsCarryingFlag() || isHeldCheck == false)
                    {
                        RetrieveState();
                    }
                    break;
                case State.Deposit:
                    if (IsCarryingFlag() || isHeldCheck == true)
                    {
                        DepositState();
                    }
                    break;
            }

            // Update the last state change time
            lastStateChangeTime = Time.time;
        }

        // Perform action based on current action
        switch (currentAction)
        {
            case Action.Move:
                Move();
                break;
            case Action.Shoot:
                Shoot();
                break;
        }
    }

    void RetrieveState()
    {
        Debug.Log("Retrieving state activated");
        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Player within attack range, shooting.");
            currentAction = Action.Shoot;
        }
        else
        {
            Debug.Log("Player not within attack range, moving.");
            currentAction = Action.Move;
        }

        // Check if the flag is nearby
        if (Vector3.Distance(transform.position, flag.position) <= 4f)
        {
            Debug.Log("Flag detected nearby.");
            // Perform action based on current objective
            if (!IsCarryingFlag())
            {
                Debug.Log("Flag not being carried, attempting to collect.");
                // Try to collect the flag
                currentAction = Action.Move;
            }
            else
            {
                Debug.Log("Flag being carried, switching to Deposit state.");
                // Return to base to deposit the flag
                currentState = State.Deposit;
            }
        }
        else
        {
            Debug.Log("Flag not detected nearby, continuing to move.");
            // Move towards the flag
            currentAction = Action.Move;
        }
    }


    void DepositState()
    {
        Debug.Log("Deposit state activated");
        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentAction = Action.Shoot;
        }
        else
        {
            currentAction = Action.Move;
        }

        // Check if the enemy is carrying the flag
        if (IsCarryingFlag())
        {
            IsCarryingFlag();
            // Move towards the base location
            currentAction = Action.Move;

            // Check if reached the base location
            if (Vector3.Distance(transform.position, baseLocation.position) <= 2f)
            {
                // Deposit the flag and switch to Retrieve state
                DepositFlag();
                currentState = State.Retrieve;
            }
            else
            {
                // Move towards the base location if not reached yet
                MoveTowardsBaseLocation();
            }
        }
        else
        {
            // Flag is dropped, evaluate actions
            // For simplicity, let's assume the enemy moves towards the flag if it's dropped
            currentAction = Action.Move;
            MoveTowardsFlag();
        }
    }

    void MoveTowardsBaseLocation()
    {
        Debug.Log("moving to Spawn");
        // Calculate direction towards the base location
        Vector3 direction = (baseLocation.position - transform.position).normalized;

        // Rotate towards the base location
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        transform.Rotate(Vector3.up, Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), turnSpeed * Time.deltaTime));

        // Move towards the base location
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    void MoveTowardsFlag()
    {
        Debug.Log("moving moving to flag");
        // Calculate direction towards the flag
        Vector3 direction = (flag.position - transform.position).normalized;

        // Rotate towards the flag
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        transform.Rotate(Vector3.up, Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), turnSpeed * Time.deltaTime));

        // Move towards the flag
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    bool IsCarryingFlag()// Checks if the flag is being held by the enemy
    {
        Debug.Log("Flag Has been collected ");
        return FlagController1.isHeld;
    }

    void DepositFlag()
    {
        
        Debug.Log("Flag deposited!");
    }

    void Move()
    {
        Debug.Log("moving");
        // Calculate direction towards the flag
        Vector3 direction = (flag.position - transform.position).normalized;

        // Rotate towards the flag
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        transform.Rotate(Vector3.up, Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), turnSpeed * Time.deltaTime));

        // Move towards the flag
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
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


