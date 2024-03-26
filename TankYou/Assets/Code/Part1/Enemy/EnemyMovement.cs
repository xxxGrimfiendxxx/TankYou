using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of movement
    public float turnSpeed = 90f; // Speed of turning (degrees per second)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        // Enemy AI movement logic
        Move();
    }

    public void Move()
    {
        MoveForward();
        Turn();

    }
    void MoveForward()
    {
        // Calculate movement
        Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;

        // Apply movement
        rb.MovePosition(rb.position + movement);
    }

    void Turn()
    {
        // Randomize turn direction
        float randomTurnDirection = Random.Range(-1f, 1f);

        // Calculate rotation
        float turnAmount = randomTurnDirection * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);

        // Apply rotation
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}

