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
    float torquedamping = 0.01f;
    float collisionDamageScale = 0.6f;

    [HideInInspector]
    public Vector3 Heading = new Vector3();

    [HideInInspector]
    public bool ReadyToSpawn = true;

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

    public float CommandTurn { get; private set;}

    public bool FireAtWill => false;

    public Vector3 DesiredThrust() => transform.forward * Time.deltaTime * CommandThrust;
    
    public Vector3 DesiredTorque()
    {
        var desiredAngle = Vector3.SignedAngle(transform.forward, Heading, Vector3.up);
        CommandTurn = Maths.Rescale(-1, 1, -180, 180, desiredAngle);
        var torqueActual = Vector3.up * desiredAngle * torquedamping * Time.deltaTime;

        // Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);
        // Debug.DrawRay(transform.position, Heading * 100, Color.green);

        return torqueActual;
    }

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
