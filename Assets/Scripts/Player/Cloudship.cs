using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloudship : MonoBehaviour, ITakeDamage, IFly
{
    public enum Modes {
        Drive,
        Build
    }

    public Modes Mode;

    public float Thrust;
    public float Turn;
    public float Health = 100;

    FlyingPhysics flyingPhysics;
    BuildSurface builder;

    void Start()
    {
        flyingPhysics = GetComponent<FlyingPhysics>();
        builder = GetComponentInChildren<BuildSurface>();
        flyingPhysics.Lift = 2000f;
        flyingPhysics.Torque = 2000f;
        flyingPhysics.Speed = 350f;
        flyingPhysics.Parent = this;

        Health = 100;

        Mode = Modes.Drive;
    }

    public void ForceMovement(Rigidbody rigidBody, float torque, float speed)
    {
        float yaw = Turn * torque * Time.deltaTime;
        rigidBody.AddTorque(transform.up * yaw);

        float forward = Thrust * speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);
    }

    public void Damage(float amount)
    {
        Health -= amount;
    }

    public void SetBuildModeOn()
    {
        Mode = Modes.Build;
        builder.enabled = true;
    }

    public void SetBuildModeOff()
    {
        Mode = Modes.Drive;
        builder.enabled = false;
    }

    public Vector3 Position
    {
        get
        {
            return this.transform.position;
        }
    }
}