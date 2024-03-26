using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBullet : MonoBehaviour
{
    public static CreateBullet instance; // Singleton instance reference
    public GameObject prefabToSpawn; // Reference to the prefab to spawn
    public float spawnDistance = 2f; // Distance in front of the player to spawn the prefab

    void Awake()
    {
        // Ensure only one instance of CreateBullet exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Shoot()
    {
        if (instance != null)
        {
            instance.ShootBullet();
        }
    }

    public void ShootBullet()
    {
        // Calculate spawn position in front of the player
        Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;

        // Spawn the prefab at the calculated position with the same rotation as the player
        Instantiate(prefabToSpawn, spawnPosition, transform.rotation);
    }
}