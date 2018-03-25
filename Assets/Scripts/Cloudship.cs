using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloudship : MonoBehaviour, ITakeDamage
{
    public GameObject cycloneForceIndicator;
    public float Thrust;
    public float Turn;
    public float Speed = 12f;
    public float Torque = 2f;
    public float Lift = 10f;
    public bool AllowMovement = true;
    public float Health = 100;

    public float StandardAltitude = 0f;

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
        if (AllowMovement)
        {
            ForceDueToPlayer();
            ForceDueToWind();
        }
        ForceDueToBouyancy();
    }

    void ForceDueToPlayer()
    {
        float yaw = Turn * Torque * Time.deltaTime;
        rigidBody.AddTorque(transform.up * yaw);

        float forward = Thrust * Speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);
    }

    void ForceDueToWind()
    {
        var cycloneForce = windMaker.GetCycloneForce(transform.position) * Time.deltaTime;
        UpdateCycloneForceIndicator(cycloneForce);
        rigidBody.AddForce(cycloneForce);   
    }

    void ForceDueToBouyancy()
    {
        var torqueAxis = Vector3.Cross(transform.up, Vector3.up) * Lift * Time.deltaTime;
        rigidBody.AddTorque(torqueAxis);

        var differenceInAltitude = 1 - (Mathf.Lerp(transform.position.y, StandardAltitude, Time.deltaTime) * 0.01f);

        var force = -Physics.gravity * rigidBody.mass * differenceInAltitude;
        rigidBody.AddForce(force);
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

    public void Damage(float amount)
    {
        Health -= amount;
    }

    public Vector3 Position
    {
        get
        {
            return this.transform.position;
        }
    }


}