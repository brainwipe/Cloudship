using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ITakeDamage
{
    public float Speed = 12f;
    public Vector3 Heading = new Vector3();
    public WeatherSystemManager weatherSystemManager;

    private Rigidbody rigidBody;
    private GameObject player;
    private Animator animator;
    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    public float Health { get; set; }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Health = 100;
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(Heading),
            Time.deltaTime);

        float forward = Speed * Time.deltaTime;
        rigidBody.AddForce(transform.forward * forward);

        var cycloneForce = weatherSystemManager.GetCycloneForce(transform.position) * Time.deltaTime;
        rigidBody.AddForce(cycloneForce);
    }

    void Update()
    {
        animator.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));
    }
}
