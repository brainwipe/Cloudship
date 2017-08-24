using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloudship : MonoBehaviour
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

        Vector3 sum = new Vector3();

        foreach(var cyclone in cyclones)
        {
            sum += cyclone.GetComponent<Anticyclone>().GetForceFor(transform.position);
        }

        return sum;
    }
}