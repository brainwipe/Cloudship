﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Cloudship : MonoBehaviour, ITakeDamage, IFly, IAmAShip, IAmATarget, IAmPersisted
{
    public enum Modes {
        Drive,
        Build
    }

    public Modes Mode;

    public float commandThrust;
    public float commandTurn;
    public float Health;
    public bool IsAlive => Health > 0;
    private float HealthMax;
    public Image HealthBar;
    public StoreManager Stores;
    FlyingPhysics flyingPhysics;
    BuildSurface builder;
    Collector collector;

    void Awake()
    {
        Stores = new StoreManager();
        flyingPhysics = GetComponent<FlyingPhysics>();
        builder = GetComponentInChildren<BuildSurface>();
        collector = GetComponentInChildren<Collector>();

        Mode = Modes.Drive;
        UpdateAbilities();
        Health = HealthMax;
    }

    void Start()
    {
        if (!GameManager.Instance.Mode.PlayerTakesDamage)
        {
            foreach(var canvas in GetComponentsInChildren<Canvas>())
            {
                canvas.enabled = false;
            }
        }
    }

    void LateUpdate() => AudioManager.Instance.SetWindFromVelocity(flyingPhysics.Blackbox.GroundSpeed);
    
    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == TerrainFactory.TerrainTag)
        {
            flyingPhysics.Grounded();
            Health = 0;
        }
    }

    public float CommandThrust => commandThrust;

    public float CommandTurn => commandTurn;
    
    public void Dead()
    {
        GameManager.Instance.End();
    }

    public void Damage(float amount) 
    {
        if (!GameManager.Instance.Mode.PlayerTakesDamage)
        {
            return;
        }

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
        commandTurn = 0f;
    }

    public void SetBuildModeOff()
    {
        Mode = Modes.Drive;
        builder.enabled = false;
    }

    public void ReelOut()
    {
        collector.ReelOut();
    }

    public void ReelIn()
    {
        collector.ReelIn();
    }
   
    public Vector3 Position => this.transform.position;

    public Vector3 Velocity => flyingPhysics.Velocity;
    
    public bool CanMove => flyingPhysics.Thrust > 0 && CanGiveOrders;
    
    public bool CanTurn => flyingPhysics.Torque > 0 && CanGiveOrders;

    public bool CanGiveOrders { get; private set; }

    public bool CanShoot => Mode != Cloudship.Modes.Build;

    public string MyEnemyTagIs => "Enemy";

    public bool ShootFullAuto => false;
    public bool FireAtWill => true;

    public bool IAmAPlayer => true;

    public bool IsDead { get; set;}

    public void UpdateAbilities()
    {
        CanGiveOrders = false;

        var buildingsWithAbility = transform.GetComponentsInChildren<IHaveAbilities>();
        HealthMax = 0;

        foreach(var building in buildingsWithAbility)
        {
            HealthMax += building.Skills.Health;
            
            if (building.Skills.GiveOrders)
            {
                CanGiveOrders = true;
            }
        }
        flyingPhysics.UpdateParameters(buildingsWithAbility, CanGiveOrders);
    }

    public void Save(SaveGame save)
    {
        save.Health = Health;
        save.HealthMax = HealthMax;
        save.Position = transform.position.ToArray();
        save.Rotation = transform.rotation.ToArray();
        save.Buildings = builder.Save();
        save.InfrastructreFlotsam = Stores.InfrastructreFlotsam;
    }

    public void Load(SaveGame save)
    {
        Health = save.Health;
        HealthMax = save.HealthMax;
        transform.position = save.Position.ToVector();
        transform.rotation = save.Rotation.ToQuaternion();
        Stores.InfrastructreFlotsam =save.InfrastructreFlotsam;
        builder.Load(save.Buildings);
        UpdateAbilities();
    }

    public void CinematicMode()
    {
        foreach(var canvas in GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = false;
        }
    }

    public void NormalMode()
    {
        if (!GameManager.Instance.Mode.PlayerTakesDamage)
        {
            return;
        }

        foreach(var canvas in GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = true;
        }
    }
}