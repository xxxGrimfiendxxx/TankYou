using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Createbullet1 : MonoBehaviour
{
    public GameObject prefabToSpawn; // Reference to the prefab to spawn
    public float spawnDistance = 2f; // Distance in front of the player to spawn the prefab

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Calculate spawn position in front of the player
        Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;

        // Spawn the prefab at the calculated position with the same rotation as the player
        Instantiate(prefabToSpawn, spawnPosition, transform.rotation);
    }
}