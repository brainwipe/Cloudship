using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlyingPhysics : MonoBehaviour
{
    public static float Vne = 50f;

    public IFly Parent;
    public bool AllowMovement;
    public bool AllowBuoyancy;
    public bool AllowWind;
    public float Lift;
    public float Torque;
    public float Speed;
    public Telemetry Blackbox;

    Rigidbody rigidBody;
    IWindMaker windMaker;

    float StandardAltitude = 0;
    float buoyancyHealth = 1f;
    bool grounded = false;
    float timeDead = 0;
    
    void Awake()
    {
        Blackbox = new Telemetry();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Parent = transform.GetComponentInParent<IFly>();
        windMaker = GameManager.Instance.WindMaker;
        rigidBody.maxAngularVelocity = 0.6f;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            timeDead += Time.deltaTime;
            if (timeDead > 5)
            {
                var sunken = new Vector3(rigidBody.transform.position.x,-220f, rigidBody.transform.position.z);
                var sinking = Vector3.Slerp(rigidBody.transform.position, sunken, Time.deltaTime * 0.1f);
                rigidBody.transform.position = sinking;
            }
            if (rigidBody.transform.position.y < -200f)
            {
                GameManager.Instance.End();
            }
            return;
        }

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
        Blackbox.Set(rigidBody.velocity);
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
        grounded = true;
    }

    public void Reset()
    {
        AllowWind = true;
        AllowMovement = true;
        AllowBuoyancy = true;
        buoyancyHealth = 1f;
        rigidBody.isKinematic = false;
        grounded = false;
        timeDead = 0;
    }

    [Serializable]
    public class Telemetry
    {
        public float Velocity;
        public float MaxVelocity;

        public void Set(Vector3 velocity)
        {
            Velocity = velocity.magnitude;
            if (Velocity > MaxVelocity)
            {
                MaxVelocity = Velocity;
            }
        }
    }
}