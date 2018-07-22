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
    
    float HealthMax;
    float torquedamping = 0.01f;

    [HideInInspector]
    public Vector3 Heading = new Vector3();

    [HideInInspector]
    public bool ReadyToSpawn = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        flyingPhysics = GetComponent<FlyingPhysics>();
    }

    void Start()
    {
        playerCloudship = GameManager.Instance.PlayerCloudship;
        UpdateAbilities();
        Health = HealthMax;
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
            flyingPhysics.Grounded();
            Health = 0;
        }
        if (collisionInfo.gameObject.tag == playerCloudship.tag)
        {
            var damageDueToCollision = collisionInfo.relativeVelocity.magnitude / 8f;
            Damage(damageDueToCollision);
            playerCloudship.Damage(damageDueToCollision);
        }
    }

    public bool IsDead => Health < 1;

    public Vector3 Position => transform.position;

    public bool CanMove => flyingPhysics.Thrust > 0 && CanGiveOrders;
    
    public bool CanTurn => flyingPhysics.Torque > 0 && CanGiveOrders;

    public bool CanGiveOrders { get; private set; }

    public bool CanShoot => true;

    public string MyEnemyTagIs => "Player";

    public bool ShootFullAuto => true;

    public bool IAmAPlayer => false;

    public float CommandThrust => 1;

    public float CommandTurn { get; set;}

    public Vector3 DesiredThrust()  => transform.forward * Time.deltaTime;
    
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
        Destroy(gameObject);
    }

    public void Damage(float amount)
    {
        Health -= amount;
        HealthBar.fillAmount = Health/HealthMax;
        if (IsDead)
        {
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
