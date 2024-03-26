using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public Transform respawnPoint; // Respawn point for automatic respawn
    public Transform Field1;
    public float respawnCooldown = 5f; // Cooldown for automatic respawn
    public KeyCode resetKey = KeyCode.R; // Key to trigger manual reset
    public float respawnHeight = 10f; // Height at which player respawns if falling

    private bool isRespawning = false;
    private float respawnTimer = 0f;

    void Update()
    {
        // Check for manual reset input
        if (Input.GetKeyDown(resetKey))
        {
            ManualReset();
        }

        // Check for automatic respawn cooldown
        if (isRespawning)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                Respawn();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check for collision with deadly objects (e.g., bottom of the level)
        if (collision.gameObject.CompareTag("deadly"))
        {
            StartRespawnCooldown();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check for trigger volume (e.g., deadly zone) enter
        if (other.CompareTag("deadly"))
        {
            StartRespawnCooldown();
        }
    }

    void ManualReset()
    {
        // Reset player position and rotation
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
    }

    void StartRespawnCooldown()
    {
        isRespawning = true;
        respawnTimer = respawnCooldown;
    }

    void Respawn()
    {
        // Reset player position and rotation
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        isRespawning = false;
    }

    void FixedUpdate()
    {
        // Check if player falls below a certain height for automatic respawn
        if (transform.position.y < -respawnHeight)
        {
            StartRespawnCooldown();
        }
    }
}