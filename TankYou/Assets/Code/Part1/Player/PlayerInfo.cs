using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class PlayerInfo : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the player
    public int currentHealth; // Current health of the player
    public TMP_Text healthText; // Reference to the TextMeshPro text for health display
    public static bool isPlayerHit = false; // Static flag to detect if the player is hit by a bullet
    public float respawnCooldown = 5f; // Cooldown duration for respawn
    public GameObject respawnPoint; // Respawn point for automatic respawn
    public float respawnHeight = 10f; // Height for respawning if player falls
    public KeyCode respawnKey = KeyCode.R; // Key to trigger manual respawn
    

    private bool isRespawning = false; // Flag to track if the player is respawning

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        UpdateHealthUI();
        if (Input.GetKeyDown(respawnKey))
        {
            // Trigger manual respawn when R key is pressed
            Reset();
        }
    }

    // Method to detect collision with a projectile
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletHit(40); // Player loses 40 health when hit by a bullet
        }
    }
    // Method to handle player being hit by a bullet
    void BulletHit(int damage)
    {
        if (!isRespawning)
        {
            currentHealth -= damage; // Decrease player's health by the damage amount
            isPlayerHit = true; // Set the flag to indicate the player is hit
            UpdateHealthUI(); // Update health UI
            if (currentHealth <= 0)
            {
                Die(); // Die if health reaches zero
            }
        }
    }

    // Method to handle player death
    void Die()
    {
        // Trigger respawn
        if (!isRespawning)
        {
            StartCoroutine(RespawnCooldown());
        }
    }

    
    // Coroutine for respawn cooldown
    IEnumerator RespawnCooldown()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnCooldown);

        if (currentHealth <= 0)
        {
            Reset();
        }

        isRespawning = false;
    }

    // Method to reset player's position and rotation
    void Reset()
    {
        if (!isRespawning)
        {
            if (currentHealth <= 0)
            {
                // Automatic respawn at respawn point if player dies
                transform.position = respawnPoint.transform.position;
                transform.rotation = Quaternion.identity;
            }
            else
            {
                // Manual respawn at higher altitude
                Vector3 respawnPos = new Vector3(transform.position.x, respawnHeight, transform.position.z);
                transform.position = respawnPos;
                transform.rotation = Quaternion.identity;
            }
            currentHealth = maxHealth; // Reset player's health
            UpdateHealthUI(); // Update health UI
        }
    }

    // Method to update health UI
    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth.ToString();
        isPlayerHit = false;
    }
}
