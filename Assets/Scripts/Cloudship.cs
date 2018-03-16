using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloudship : MonoBehaviour, ITakeDamage
{
    public GameObject cycloneForceIndicator;
    public float Thrust;
    public float Turn;
    public float velocity;
    public float Speed = 12f;
    public float Torque = 2f;
    public float MaxVelocity = 0.4f;
    public float Health { get; set; }

    Rigidbody rigidBody;
    IWindMaker windMaker;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        windMaker = GameManager.Instance.WindMaker;
        rigidBody.maxAngularVelocity = 0.6f;
        Health = 100;
    }

    void FixedUpdate()
    {
        float yaw = Turn * Torque * Time.deltaTime;
        rigidBody.AddTorque(transform.up * yaw);

        float forward = Thrust * Speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);

        var cycloneForce = windMaker.GetCycloneForce(transform.position) * Time.deltaTime;
        UpdateCycloneForceIndicator(cycloneForce);
        rigidBody.AddForce(cycloneForce);   
    }

    void UpdateCycloneForceIndicator(Vector3 cycloneForce)
    {
        if (cycloneForce.magnitude == 0)
        {
            cycloneForceIndicator.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            return;
        }

        var windDirection = Quaternion.LookRotation(-cycloneForce);

        cycloneForceIndicator.transform.rotation = Quaternion.Slerp(
            cycloneForceIndicator.transform.rotation, 
            windDirection,
            Time.deltaTime * 1.5f);
    }

    public Vector3 Position
    {
        get
        {
            return this.transform.position;
        }
    }


}