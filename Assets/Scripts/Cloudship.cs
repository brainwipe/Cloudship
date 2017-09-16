using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloudship : MonoBehaviour
{
    public WeatherSystemManager weatherSystemManager;
    public GameObject cycloneForceIndicator;
    private Rigidbody rigidBody;
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

        var cycloneForce = weatherSystemManager.GetCycloneForce(transform.position) * Time.deltaTime;
        UpdateCycloneForceIndicator(cycloneForce);
        rigidBody.AddForce(cycloneForce);
    }

    void UpdateCycloneForceIndicator(Vector3 cycloneForce)
    {
        if (cycloneForce.magnitude == 0)
        {
            cycloneForceIndicator.transform.rotation = new Quaternion(0f,0f,0f,0f);
            return;
        }
        cycloneForceIndicator.transform.rotation = Quaternion.LookRotation(cycloneForce);
        cycloneForceIndicator.transform.localScale.Set(0.1f, 1f, cycloneForce.magnitude);
    }

    
}