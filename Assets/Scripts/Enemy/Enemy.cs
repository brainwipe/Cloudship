using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ITakeDamage
{
    public float Speed = 6f;
    public float Lift = 12f;
    public float StandardAltitude = 0f;
    public Vector3 Heading = new Vector3();

    public bool AllowMovementForce = true;
    public bool AllowWindForce = true;
    public bool AllowBuoyancy = true;

    IWindMaker windMaker;
    Rigidbody rigidBody;
    Cloudship playerCloudship;
    Animator animator;

    public float Distance;

    public float Health = 0;
    public float BuoyancyHealth = 1f;
    public bool ReadyToSpawn = true;

    float timeDead;
    bool grounded = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        windMaker = GameManager.Instance.WindMaker;
        playerCloudship = GameManager.Instance.PlayerCloudship;
    }

    void FixedUpdate()
    {
        ForceDueToHeading();
        ForceDueToWind();
        ForceDueToBuoyancy();
        Debug.DrawRay(transform.position, rigidBody.velocity, Color.red);
    }

    void ForceDueToHeading()
    {
        if (!AllowWindForce)
        {
            return;
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(Heading),
            Time.deltaTime);

        float forward = Speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);
    }

    void ForceDueToWind()
    {
        if (!AllowWindForce)
        {
            return;
        }

        var cycloneForce = windMaker.GetCycloneForce(transform.position) * Time.deltaTime;
        rigidBody.AddForce(cycloneForce);
    }

    void ForceDueToBuoyancy()
    {
        if (!AllowBuoyancy)
        {
            return;
        }
        var torqueAxis = Vector3.Cross(transform.up, Vector3.up) * Lift * Time.deltaTime * BuoyancyHealth;
        rigidBody.AddTorque(torqueAxis);

        var differenceInAltitude = 1 - (Mathf.Lerp(transform.position.y, StandardAltitude, Time.deltaTime) * 0.01f);

        var force = -Physics.gravity * rigidBody.mass * BuoyancyHealth * differenceInAltitude;
        rigidBody.AddForce(force);
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
                AllowBuoyancy = false;
                rigidBody.isKinematic = true;
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
            AllowBuoyancy = false;
            Health = 0;
            grounded = true;
        }
        if (collisionInfo.gameObject.tag == playerCloudship.tag)
        {
            var damageDueToCollision = collisionInfo.relativeVelocity.sqrMagnitude;
            Damage(damageDueToCollision);
            playerCloudship.Damage(damageDueToCollision);
        }
    }

    public bool IsDead 
    {
        get {
            return Health < 1;
        }
    }

    public void Damage(float amount)
    {
        Health -= amount;
        if (IsDead)
        {
            Debug.Log("Enemy Dead");
            Speed = 0;
            AllowMovementForce = false;
            AllowWindForce = false;
            BuoyancyHealth = 0.2f;

            var shooting = GetComponent<Shooting>();
            shooting.enabled = false;
        }
    }

    public void Reset()
    {
        if (!ReadyToSpawn)
        {
            return;
        }
        Health = 50;

        var newLocation = GameManager.Instance.PlayerCloudship.transform.position;
        newLocation.x = newLocation.x + 2000f;    
    
        transform.position = newLocation;

        timeDead = 0;
        grounded = false;
        AllowMovementForce = true;
        AllowBuoyancy = true;
        AllowWindForce = true;
        BuoyancyHealth = 1f;
        rigidBody.isKinematic = false;

        var shooting = GetComponent<Shooting>();
        shooting.enabled = true;
        ReadyToSpawn = false;
        
    }
}
