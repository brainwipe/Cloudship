using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlyingPhysics : MonoBehaviour
{
    public static float Vne = 70f;

    public IFly Parent;
    public bool AllowMovement;
    public bool AllowBuoyancy;
    public bool AllowWind;
    public float Lift;
    public float Torque;
    public float Thrust;
    [HideInInspector]
    public Vector3 ThrustForce;
    public float TopSpeed;
    [HideInInspector]
    public Vector3 CycloneForce = Vector3.zero; 
    public float WindSpeed = 0f;
    public float IndicatedAirSpeed = 0f;
    public Telemetry Blackbox;

    Rigidbody rigidBody;
    IWindMaker windMaker;
    

    float StandardAltitude = 0;
    Vector3 lastVelocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    Vector3 windVector = Vector3.zero;
    Vector3 thrustVelocity = Vector3.zero;
    float rockAndRollForce = 600f;
    float formDragCoefficient = 70f;
    float buoyancyHealth = 1f;
    bool grounded = false;
    float timeDead = 0;
    
    void Awake()
    {
        Blackbox = new Telemetry();
        rigidBody = transform.GetComponent<Rigidbody>();
        Parent = transform.GetComponent<IFly>();
    }

    void Start()
    {
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
                Parent.Dead();
            }
            return;
        }

        if (AllowMovement)
        {
            ThrustForce = Parent.DesiredThrust() * Thrust;
            rigidBody.AddForce(ThrustForce);
            rigidBody.AddTorque(Parent.DesiredTorque() * Torque);
        }
        if (AllowWind)
        {
            ForceDueToWind();
        }
        if (AllowBuoyancy)
        {
            ForceDueToBouyancy();
        }

        CalculateDerivedValues();
        ForceDueToFormDrag();
        ForceDueToRockAndRoll();

        Blackbox.Set(rigidBody.velocity, acceleration);
    }

    void CalculateDerivedValues()
    {
        acceleration = (rigidBody.velocity - lastVelocity) / Time.deltaTime;
        lastVelocity = rigidBody.velocity;
        windVector = CycloneForce / (Mass * Time.deltaTime * 10f);
        WindSpeed = windVector.magnitude;
        thrustVelocity = (rigidBody.velocity - windVector);
        IndicatedAirSpeed = thrustVelocity.magnitude;
    }

    void ForceDueToWind()
    {
        CycloneForce = windMaker.GetCycloneForce(transform.position) * Time.deltaTime;
        rigidBody.AddForce(CycloneForce);   
    }

    void ForceDueToBouyancy()
    {
        var torqueAxis = Vector3.Cross(transform.up, Vector3.up) * Lift * Time.deltaTime * buoyancyHealth;
        rigidBody.AddTorque(torqueAxis);

        var differenceInAltitude = 1 - (Mathf.Lerp(transform.position.y, StandardAltitude, Time.deltaTime) * 0.01f);

        var force = -Physics.gravity * rigidBody.mass * buoyancyHealth * differenceInAltitude;
        rigidBody.AddForce(force);
    }

    void ForceDueToRockAndRoll()
    {
        var rockAndRollAxis = Vector3.Cross(acceleration, transform.up);
        rigidBody.AddTorque(rockAndRollAxis * rockAndRollForce);
    }

    void ForceDueToFormDrag()
    {
        if (TopSpeed < 1)
        {
            return;
        }
        var velocityDueToThrust = (rigidBody.velocity - windVector);
        var amountOverTopSpeed = IndicatedAirSpeed - TopSpeed;
        
        if (amountOverTopSpeed > 0)
        {
            Debug.DrawRay(transform.position, -velocityDueToThrust * 10, Color.black, 0.1f);
            rigidBody.AddForce(-velocityDueToThrust * formDragCoefficient);
        }
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

    public void UpdateParameters(IHaveAbilities[] abilities, bool canGiveOrders)
    {
        Torque = 0;
        Thrust = 0;
        Mass = 0;
        TopSpeed = 0;

        float topSpeedEfficiencyMultiplier = 1f; 

        foreach(var ability in abilities)
        {
            Torque += ability.Skills.Torque;
            Thrust += ability.Skills.Thrust;
            Lift += ability.Skills.Lift;
            Mass += ability.Skills.Mass;

            if (ability.Skills.TopSpeed > 0)
            {
                TopSpeed += ability.Skills.TopSpeed * topSpeedEfficiencyMultiplier;
                topSpeedEfficiencyMultiplier = topSpeedEfficiencyMultiplier * 0.8f;
            }
            
        }

        if (!canGiveOrders)
        {
            Torque = 0;
            Thrust = 0;
        }
    }

    public float Mass 
    {
        get
        {
            return rigidBody.mass;
        }
        set
        {
            rigidBody.mass = value;
        }
    }

    public bool IsAdrift => TopSpeed == 0;

    [Serializable]
    public class Telemetry
    {
        float lastVelocity;

        public float Velocity;
        public float MaxVelocity;
        public float Acceleration;

        public void Set(Vector3 velocity, Vector3 acceleration)
        {
            Velocity = velocity.magnitude;
            if (Velocity > MaxVelocity)
            {
                MaxVelocity = Velocity;
            }
            Acceleration = acceleration.magnitude;
            
        }
    }
}