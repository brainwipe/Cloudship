using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPhysics : MonoBehaviour
{
    public IFly Parent;
    public bool AllowMovement;
    public bool AllowBuoyancy;
    public bool AllowWind;
    public float Lift;
    public float Torque;
    public float Speed;

    Rigidbody rigidBody;
    IWindMaker windMaker;

    float StandardAltitude = 0;
    float buoyancyHealth = 1f;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        windMaker = GameManager.Instance.WindMaker;
        rigidBody.maxAngularVelocity = 0.6f;
    }

    void FixedUpdate()
    {
        if (AllowMovement)
        {
            Parent.ForceMovement(rigidBody, Torque, Speed);
        }
        if (AllowWind)
        {
        ForceDueToWind();
        }
        if (AllowBuoyancy)
        {
            ForceDueToBouyancy();
        }
    }

    void ForceDueToWind()
    {
        var cycloneForce = windMaker.GetCycloneForce(transform.position) * Time.deltaTime;
        rigidBody.AddForce(cycloneForce);   
    }

    void ForceDueToBouyancy()
    {
        var torqueAxis = Vector3.Cross(transform.up, Vector3.up) * Lift * Time.deltaTime * buoyancyHealth;
        rigidBody.AddTorque(torqueAxis);

        var differenceInAltitude = 1 - (Mathf.Lerp(transform.position.y, StandardAltitude, Time.deltaTime) * 0.01f);

        var force = -Physics.gravity * rigidBody.mass * buoyancyHealth * differenceInAltitude;
        rigidBody.AddForce(force);
    }

    public void SinkToGround()
    {
        AllowMovement = false;
        AllowWind = false;
        AllowBuoyancy = true;
        buoyancyHealth = 0.2f;
    }

    public void Grounded()
    {
        AllowBuoyancy = false;
        rigidBody.isKinematic = true;
    }

    public void Reset()
    {
        AllowWind = true;
        AllowMovement = true;
        AllowBuoyancy = true;
        buoyancyHealth = 1f;
        rigidBody.isKinematic = false;
    }

}