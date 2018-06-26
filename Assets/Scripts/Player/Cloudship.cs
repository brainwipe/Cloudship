using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloudship : MonoBehaviour, ITakeDamage, IFly, IAmAShip, IAmATarget, IAmPersisted
{
    public enum Modes {
        Drive,
        Build
    }

    public Modes Mode;

    public float Thrust;
    public float Turn;
    public float Health;
    public bool IsAlive => Health > 0;
    private float HealthMax;
    public Image HealthBar;
    FlyingPhysics flyingPhysics;
    BuildSurface builder;

    void Awake()
    {
        flyingPhysics = GetComponent<FlyingPhysics>();
        builder = GetComponentInChildren<BuildSurface>();

        Mode = Modes.Drive;
        UpdateAbilities();
    }

    void LateUpdate() => AudioManager.Instance.SetWindFromVelocity(flyingPhysics.Blackbox.Velocity);

    public void ForceMovement(Rigidbody rigidBody, float torque, float speed)
    {
        float yaw = Turn * torque * Time.deltaTime;
        rigidBody.AddTorque(transform.up * yaw);

        float forward = Thrust * speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);
    }

    public void Dead()
    {
        GameManager.Instance.End();
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == TerrainFactory.TerrainTag)
        {
            flyingPhysics.Grounded();
            Health = 0;
        }
    }

    public void Damage(float amount) 
    {
        Health -= amount;
        HealthBar.fillAmount = Health/HealthMax;

        if (Health < 1)
        {
            Die();
        }
    }

    private void Die()
    {
        flyingPhysics.SinkToGround();
        IsDead = true;
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

    public Vector3 Position => this.transform.position;
    
    public bool CanMove => flyingPhysics.Speed > 0 && CanGiveOrders;
    
    public bool CanTurn => flyingPhysics.Torque > 0 && CanGiveOrders;

    public bool CanGiveOrders { get; private set; }

    public bool CanShoot => Mode != Cloudship.Modes.Build;

    public string MyEnemyTagIs => "Enemy";

    public bool ShootFullAuto => false;

    public bool IAmAPlayer => true;

    public bool IsDead { get; set;}

    public void UpdateAbilities()
    {
        CanGiveOrders = false;

        var buildingsWithAbility = transform.GetComponentsInChildren<IHaveAbilities>();

        flyingPhysics.Torque = 0;
        flyingPhysics.Speed = 0;
        float mass = 0f;
        Health = 0;

        foreach(var building in buildingsWithAbility)
        {
            flyingPhysics.Torque += building.Skills.Torque;
            flyingPhysics.Speed += building.Skills.Speed;
            flyingPhysics.Lift += building.Skills.Lift;
            Health += building.Skills.Health;
            mass += building.Skills.Mass;
            
            if (building.Skills.GiveOrders)
            {
                CanGiveOrders = true;
            }
        }

        if (!CanGiveOrders)
        {
            flyingPhysics.Torque = 0;
            flyingPhysics.Speed = 0;
        }

        flyingPhysics.Mass = mass;
        HealthMax = Health;
    }

    public void Save(SaveGame save)
    {
        save.Health = Health;
        save.HealthMax = HealthMax;
        save.Position = transform.position.ToArray();
        save.Rotation = transform.rotation.ToArray();
        save.Buildings = builder.Save();
        Debug.Log(" save building count:" + save.Buildings.Count);
    }

    public void Load(SaveGame save)
    {
        Health = save.Health;
        HealthMax = save.HealthMax;
        transform.position = save.Position.ToVector();
        transform.rotation = save.Rotation.ToQuaternion();
        builder.Load(save.Buildings);
        Debug.Log(" load building count:" + save.Buildings.Count);
        UpdateAbilities();
    }
}