using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagController1 : MonoBehaviour
{
    public Transform enemy; // Reference to the enemy GameObject
    public Transform playerBase; // Reference to the player's base location
    public float teleportDistance = 10f; // Distance from player base to trigger teleport
    public Collider pointColliderEnemy; // Collider for enemy's scoring zone
    public float cooldownDuration = 3f; // Cooldown duration after flag is dropped
    public bool isEnemyHitRead = false; // Flag to detect if enemy is hit with a bullet from enemy ai script
    public static bool isHeld = false; // global var to check if the flag is held
    public static int EnemyScore;
    public TMP_Text textbox;

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Update()
    {
        isEnemyHitRead = EnemyAI.isEnemyHit;
        textbox.text = EnemyScore.ToString();

        if (!isCooldown)
        {
            if (isHeld)
            {
                // Follow the enemy if held
                transform.position = enemy.position;

                // Check if the enemy is hit with a bullet
                if (isEnemyHitRead)
                {
                    DropFlag();
                }
            }
            else
            {
                // Check if the flag is away from the player base and collided with player
                if (Vector3.Distance(transform.position, playerBase.position) > teleportDistance)
                {
                    // Teleport the flag to player base
                    transform.position = playerBase.position;
                }
            }
        }
        else
        {
            // Handle cooldown
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isHeld && other.gameObject.CompareTag("Enemy"))
        {
            PickUp();
        }
        else if (other == pointColliderEnemy)
        {
            ScorePoint();
        }
    }

    void PickUp()
    {
        isHeld = true;
        transform.parent = enemy; // Make the flag a child of the enemy to follow its movement
    }

    void DropFlag()
    {
        isHeld = false;
        isEnemyHitRead = false;
        transform.parent = null;
        // Start cooldown
        isCooldown = true;
        cooldownTimer = cooldownDuration;
    }

    void ScorePoint()
    {
        EnemyScore++; 
        isHeld = false; // Reset flag pickup state
        transform.parent = null; // Ensure the flag is not a child of any object
        transform.position = playerBase.position; //sends flag back to base
        
    }
}
