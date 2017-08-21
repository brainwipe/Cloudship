using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float thrust;
    public float turn;
    public float velocity;
    public float Speed = 10f;
    public float Torque = 10f;
    public float MaxVelocity = 0.4f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.maxAngularVelocity = 0.6f;
    }

    void Update()
    {
        thrust = Input.GetAxis("Vertical");
        turn = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        float yaw = turn * Torque * Time.deltaTime;
        rigidBody.AddTorque(transform.up * yaw);

        float forward = thrust * Speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);

        velocity = rigidBody.velocity.sqrMagnitude;
        if (velocity > MaxVelocity)
        {
            rigidBody.AddForce(transform.forward * (MaxVelocity - velocity));
        }
    }
}