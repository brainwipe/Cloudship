using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ITakeDamage
{
    public float Speed = 6f;
    public float Lift = 12f;
    public Vector3 Heading = new Vector3();

    public bool AllowMovementForce = true;
    public bool AllowWindForce = true;
    public bool AllowBuoyancy = true;

    IWindMaker windMaker;
    Rigidbody rigidBody;
    Cloudship playerCloudship;
    Animator animator;

    public float Distance;

    public float Health = 50;
    public float BuoyancyHealth = 1f;

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

        var force = -Physics.gravity * rigidBody.mass * BuoyancyHealth;
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
            if (timeDead > 7)
            {
                rigidBody.isKinematic = true;
                var sunken = new Vector3(0,-5,0);
                var sinking = Vector3.Slerp(transform.position, sunken, Time.deltaTime * 0.03f);
                transform.position = sinking;
            }

            if (transform.position.y < -4.9f)
            {
                // Reset!
                Debug.Log("Reset");
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == TerrainFactory.TerrainTag)
        {
            AllowBuoyancy = false;
            Health = 0;
            grounded = true;
            BuoyancyHealth = 0.99f;
        }
    }

    bool IsDead 
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
            Debug.Log("Dead");
            Speed = 0;
            AllowMovementForce = false;
            AllowWindForce = false;
            BuoyancyHealth = 0.95f;

            var shooting = GetComponent<Shooting>();
            shooting.enabled = false;
        }
    }
}
