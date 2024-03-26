using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagControllerPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player GameObject
    public Transform enemyBase; // Reference to the enemy's base location
    public float teleportDistance = 10f; // Distance from enemy base to trigger teleport
    public Collider pointColliderPlayer; // Collider for player's scoring zone
    public float cooldownDuration = 3f; // Cooldown duration after flag is dropped
    public bool isPlayerHitRead = false; // Flag to detect if player is hit with a bullet
    public static bool isHeld = false; // Global variable to check if the flag is held
    public static int PlayerScore;
    public TMP_Text textbox;

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Update()
    {
        isPlayerHitRead = PlayerInfo.isPlayerHit;
        textbox.text = PlayerScore.ToString();

        if (!isCooldown)
        {
            if (isHeld)
            {
                // Follow the player if held
                transform.position = player.position;

                // Check if the player is hit with a bullet
                if (isPlayerHitRead)
                {
                    DropFlag();
                }
            }
            else
            {
                // Check if the flag is away from the enemy base and collided with the enemy
                if (Vector3.Distance(transform.position, enemyBase.position) > teleportDistance)
                {
                    // Teleport the flag to enemy base
                    transform.position = enemyBase.position;
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
        if (!isHeld && other.gameObject.CompareTag("Player"))
        {
            PickUp();
        }
        else if (other == pointColliderPlayer)
        {
            ScorePoint();
        }
    }

    void PickUp()
    {
        isHeld = true;
        transform.parent = player; // the flag is now a child of the player to follow its movement
    }

    void DropFlag()
    {
        isHeld = false;
        isPlayerHitRead = false;
        transform.parent = null; // Reset the flag's parent to null
                                 // Start cooldown
        isCooldown = true;
        cooldownTimer = cooldownDuration;
    }


    void ScorePoint()
    {
        PlayerScore++;
        isHeld = false; // Reset flag pickup state
        transform.parent = null; // Ensure the flag is not a child of any object
        transform.position = enemyBase.position; // Respawn the flag at enemy base
    }
}