using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ITakeDamage
{
    public float Speed = 12f;
    public Vector3 Heading = new Vector3();

    IWindMaker windMaker;
    Rigidbody rigidBody;
    Cloudship playerCloudship;
    Animator animator;

    void Awake()
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
        animator.SetFloat("distance", Vector3.Distance(transform.position, playerCloudship.Position));
    }
}
