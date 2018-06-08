using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, ITakeDamage, IFly, IAmAShip
{
    public float Health = 0;
    public float Distance;

    Cloudship playerCloudship;
    Animator animator;
    public Image HealthBar;
    FlyingPhysics flyingPhysics;
    
    float HealthMax = 0;
    float timeDead;
    bool grounded = false;

    [HideInInspector]
    public Vector3 Heading = new Vector3();

    [HideInInspector]
    public bool ReadyToSpawn = true;

    void Start()
    {
        animator = GetComponent<Animator>();

        flyingPhysics = GetComponent<FlyingPhysics>();
        flyingPhysics.Lift = 2000f;
        flyingPhysics.Torque = 2200f;
        flyingPhysics.Speed = 500f;
        flyingPhysics.Parent = this;

        playerCloudship = GameManager.Instance.PlayerCloudship;
        HealthMax = Health;
    }

    public void ForceMovement(Rigidbody rigidBody, float torque, float speed)
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(Heading),
            Time.deltaTime);

        float forward = speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);
    }

    void Update()
    {
        Distance = Vector3.Distance(transform.position, playerCloudship.Position);
        animator.SetFloat("distance", Distance);
        animator.SetFloat("health", Health);

        if (grounded)
        {
            timeDead += Time.deltaTime;
            if (timeDead > 5)
            {
                var sunken = new Vector3(transform.position.x,-220f, transform.position.z);
                var sinking = Vector3.Slerp(transform.position, sunken, Time.deltaTime * 0.1f);
                transform.position = sinking;
            }

            if (transform.position.y < -180)
            {   
                ReadyToSpawn = true;
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == TerrainFactory.TerrainTag)
        {
            flyingPhysics.Grounded();
            Health = 0;
            grounded = true;
        }
        if (collisionInfo.gameObject.tag == playerCloudship.tag)
        {
            var damageDueToCollision = collisionInfo.relativeVelocity.magnitude / 8f;
            Damage(damageDueToCollision);
            playerCloudship.Damage(damageDueToCollision);
        }
    }

    public bool IsDead => Health < 1;

    public void Damage(float amount)
    {
        Health -= amount;
        HealthBar.fillAmount = Health/HealthMax;
        if (IsDead)
        {
            Debug.Log("Enemy Dead");
            flyingPhysics.SinkToGround();

            var shooting = GetComponent<Shooting>();
            shooting.enabled = false;
            HealthBar.enabled = false;
        }
    }

    public void Reset()
    {
        if (!ReadyToSpawn)
        {
            return;
        }
        Health = HealthMax;

        var newLocation = GameManager.Instance.PlayerCloudship.transform.position;
        newLocation.x = newLocation.x + 2000f;    
        transform.position = newLocation;

        timeDead = 0;
        grounded = false;
        
        if (flyingPhysics != null)
        {
            flyingPhysics.Reset();
        }

        var shooting = GetComponent<Shooting>();
        shooting.enabled = true;
        ReadyToSpawn = false;
        HealthBar.enabled = true;
        HealthBar.fillAmount = Health/HealthMax;
        
    }
}
