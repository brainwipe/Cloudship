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

    public bool CanMove => flyingPhysics.Speed > 0 && CanGiveOrders;
    
    public bool CanTurn => flyingPhysics.Torque > 0 && CanGiveOrders;

    public bool CanGiveOrders { get; private set; }

    public bool CanShoot => true;

    public string MyEnemyTagIs => "Player";

    public bool ShootFullAuto => true;

    public bool IAmAPlayer => false;

    public void ForceMovement(Rigidbody rigidBody, float torque, float speed)
    {
        float yaw = Time.deltaTime * torque;
        var desired = Vector3.SignedAngle(transform.forward, Heading, Vector3.up);

        var torqueActual = Vector3.up * desired * yaw * torquedamping;

        Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);
        Debug.DrawRay(transform.position, Heading * 100, Color.green);

        rigidBody.AddTorque(torqueActual, ForceMode.Acceleration);

        float thrust = speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * thrust);
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
}
