using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ITakeDamage
{
    public float Speed = 6f;
    public Vector3 Heading = new Vector3();

    IWindMaker windMaker;
    Rigidbody rigidBody;
    Cloudship playerCloudship;
    Animator animator;

    public float Distance;

    void Start()
    {
        
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        windMaker = GameManager.Instance.WindMaker;
        playerCloudship = GameManager.Instance.PlayerCloudship;
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(Heading),
            Time.deltaTime);

        float forward = Speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);

        var cycloneForce = windMaker.GetCycloneForce(transform.position) * Time.deltaTime;
        rigidBody.AddForce(cycloneForce);
    }

    void Update()
    {
        Distance = Vector3.Distance(transform.position, playerCloudship.Position);

        animator.SetFloat("distance", Distance);
    }
}
