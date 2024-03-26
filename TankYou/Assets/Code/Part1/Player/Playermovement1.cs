using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement1 : MonoBehaviour
{
    public float moveSpeed = 7f;  // Speed of movement
    public float turnSpeed = 35f; // Speed of turning (degrees per second)

    private Rigidbody rb;
    private float forwardInput;
    private float rotationInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Input for forward and backward movement
        forwardInput = Input.GetAxis("Vertical");//"Vertical"

        // Input for rotation
        rotationInput = Input.GetAxis("Horizontal");//"Horizontal"
    }

    void FixedUpdate()
    {
        MoveForwardBackward();
        Turn();
    }

    void MoveForwardBackward()
    {
        // Calculate movement
        Vector3 movement = transform.forward * forwardInput * moveSpeed * Time.deltaTime;

        // Apply movement
        rb.MovePosition(rb.position + movement);
    }

    void Turn()
    {
        // Calculate rotation
        float turnAmount = rotationInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);

        // Apply rotation
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}