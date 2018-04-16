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
    Shooting shooting;

    void Start()
    {
        flyingPhysics = GetComponent<FlyingPhysics>();
        builder = GetComponentInChildren<BuildSurface>();
        shooting = GetComponent<Shooting>();
        flyingPhysics.Lift = 2000f;
        flyingPhysics.Torque = 2500f;
        flyingPhysics.Speed = 500f;
        flyingPhysics.Parent = this;

        Health = 100;

        Mode = Modes.Drive;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            builder.enabled = !builder.enabled;
            shooting.enabled = !shooting.enabled;
        }
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

    public Vector3 Position
    {
        get
        {
            return this.transform.position;
        }
    }
}