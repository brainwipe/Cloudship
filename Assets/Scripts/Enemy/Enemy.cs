using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private GameObject player;

    private Animator animator;

    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        animator.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));
    }
}
