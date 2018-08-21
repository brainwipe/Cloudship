using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlyingPhysics : MonoBehaviour
{
    public static float Vne = 70f;

    public bool AllowMovement;
    public bool AllowBuoyancy;
    public bool AllowWind;
    public float Lift;
    public float Torque;
    public float Thrust;
    public float TopSpeed;
    [HideInInspector]
    public Vector3 CycloneForce = Vector3.zero; 
    public float IndicatedAirSpeed = 0f;
    public Telemetry Blackbox;

    Rigidbody rigidBody;
    IWindMaker windMaker;
    IFly parent;
    

    Vector3 thrustForce;
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
    float jinkedTorqueYawThreshold = 0.05f;
    float jinkedTorqueMultiplier = 30f;

    Dictionary<int, float> TopSpeedDiminishingReturn = new Dictionary<int, float> {
        { 0, 1f }, { 1, 0.5f }, { 2, 0.3f }, { 3, 0.3f }, { 4, 0.3f }, { 5, 0.2f }, { 6, 0.2f }
    };
    
    void Awake()
    {
        Blackbox = new Telemetry();
        rigidBody = transform.GetComponent<Rigidbody>();
        parent = transform.GetComponent<IFly>();
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
                parent.Dead();
            }
            return;
        }

        if (AllowMovement)
        {
            ForceDueToCommandThrust();
            ForceDueToCommandTurn();
            
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

        Blackbox.Set(rigidBody.velocity, acceleration, windVector);
    }

    void CalculateDerivedValues()
    {
        acceleration = (rigidBody.velocity - lastVelocity) / Time.deltaTime;
        lastVelocity = rigidBody.velocity;
        windVector = CycloneForce / (Mass * Time.deltaTime * 10f);
        thrustVelocity = (rigidBody.velocity - windVector);
        IndicatedAirSpeed = thrustVelocity.magnitude;
    }

    void ForceDueToCommandThrust()
    {
        rigidBody.AddForce(transform.forward * parent.CommandThrust * Thrust * Time.deltaTime);
    }

    void ForceDueToCommandTurn()
    {
        if (parent.CommandTurn == 0)
        {
            float direction = 1;
            if (rigidBody.angularVelocity.y > 0)
            {
                direction = -1;
            }
            rigidBody.AddTorque(transform.up * Time.deltaTime * Torque * direction);
        }
        else
        {
            var jinkedTorque = Torque;
            if (Mathf.Abs(rigidBody.angularVelocity.y) < jinkedTorqueYawThreshold)
            {
                jinkedTorque = jinkedTorque * jinkedTorqueMultiplier;

            }
            rigidBody.AddTorque(transform.up * parent.CommandTurn * Time.deltaTime * jinkedTorque);
        }
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

        var boilerCount = 0;

        foreach(var ability in abilities)
        {
            Torque += ability.Skills.Torque;
            Thrust += ability.Skills.Thrust;
            Lift += ability.Skills.Lift;
            Mass += ability.Skills.Mass;

            if (ability.Skills.TopSpeed > 0)
            {
                TopSpeed += ability.Skills.TopSpeed * TopSpeedDiminishingReturn[boilerCount];
                boilerCount ++;
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

    public Vector3 Velocity => rigidBody.velocity;
    
    public bool IsAdrift => TopSpeed == 0;

    [Serializable]
    public class Telemetry
    {
        public float GroundSpeed;
        public float MaxGroundSpeed;
        public float Acceleration;
        public float WindSpeed = 0f;

        public void Set(Vector3 velocity, Vector3 acceleration, Vector3 windVector)
        {
            GroundSpeed = velocity.magnitude;
            if (GroundSpeed > MaxGroundSpeed)
            {
                MaxGroundSpeed = GroundSpeed;
            }
            Acceleration = acceleration.magnitude;
            WindSpeed = windVector.magnitude;
            
        }
    }
}