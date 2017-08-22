using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private float cycloneEffect = 1f;
    public float thrust;
    public float turn;
    public float velocity;
    public float Speed = 12f;
    public float Torque = 2f; 
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

        var cycloneForce = GetCycloneForce() * Time.deltaTime;
        rigidBody.AddForce(cycloneForce * cycloneEffect);
    }

    Vector3 GetCycloneForce()
    {
        var cyclones = GameObject.FindGameObjectsWithTag("WeatherSystem");

        // TODO ROLA - change this to sum for many when we have more than one
        var single = cyclones[0].GetComponent<Anticyclone>();
        return single.GetForceFor(transform.position);
    }
}