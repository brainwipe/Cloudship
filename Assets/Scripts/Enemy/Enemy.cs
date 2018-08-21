using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, ITakeDamage, IFly, IAmAShip, IAmATarget
{
    public float Health;
    public float Distance;
    
    Cloudship playerCloudship;
    Animator animator;
    public Image HealthBar;
    FlyingPhysics flyingPhysics;
    EnemyFactory enemyFactory;
    
    float HealthMax;
    float collisionDamageScale = 0.6f;

    [HideInInspector]
    public Vector3 Heading = new Vector3();

    void Awake()
    {
        animator = GetComponent<Animator>();
        flyingPhysics = GetComponent<FlyingPhysics>();
        enemyFactory = GetComponentInParent<EnemyFactory>();
    }

    void Start()
    {
        playerCloudship = GameManager.Instance.PlayerCloudship;
        UpdateAbilities();
        Health = HealthMax;
        CommandThrust = 1;
    }

    void Update()
    {
        Distance = Vector3.Distance(transform.position, playerCloudship.Position);
        animator.SetFloat("distance", Distance);
        animator.SetFloat("health", Health);
        animator.SetBool("playerAlive", playerCloudship.IsAlive);
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == TerrainFactory.TerrainTag)
        {
            CreateFlotsam();
            flyingPhysics.Grounded();
            Health = 0;
        }
        if (collisionInfo.gameObject.tag == playerCloudship.tag)
        {
            var damageDueToCollision = collisionInfo.relativeVelocity.magnitude * collisionDamageScale;
            Damage(damageDueToCollision);
            playerCloudship.Damage(damageDueToCollision);
        }
    }

    private void CreateFlotsam()
    {
        var flotsam = GameManager.Instance.TerrainFactory.CreateFlotsamAt(Position);
        flotsam.Value = 200;
    }

    public bool IsDead => Health < 1;

    public Vector3 Position => transform.position;

    public Vector3 Velocity => flyingPhysics.Velocity;

    public bool CanMove => flyingPhysics.Thrust > 0 && CanGiveOrders;
    
    public bool CanTurn => flyingPhysics.Torque > 0 && CanGiveOrders;

    public bool CanGiveOrders { get; private set; }

    public bool CanShoot => true;

    public string MyEnemyTagIs => "Player";

    public bool ShootFullAuto => true;

    public bool IAmAPlayer => false;

    public float CommandThrust { get; private set;}

    public float CommandTurn => Maths.Rescale(-1, 1, -180, 180, Vector3.SignedAngle(transform.forward, Heading, Vector3.up));

    public bool FireAtWill => false;

    public void Dead()
    {
        enemyFactory.ResetTimer();
        Destroy(gameObject);
    }

    public void Damage(float amount)
    {
        Health -= amount;
        HealthBar.fillAmount = Health/HealthMax;
        if (IsDead)
        {
            CommandThrust = 0;
            flyingPhysics.SinkToGround();
            HealthBar.enabled = false;
        }
    }

    public void UpdateAbilities()
    {
        CanGiveOrders = false;

        var buildingsWithAbility = GetComponentsInChildren<IHaveAbilities>();
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
}
