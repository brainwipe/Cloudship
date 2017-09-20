using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody cannonball;
    public Transform cannon;

    private string fireButton = "Fire1";
    private bool fired;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonUp(fireButton))
        {
			Rigidbody ball = Instantiate(cannonball, cannon.position, cannon.rotation) as Rigidbody;
			ball.velocity = cannon.forward * 10;
			var cannonBall = ball.GetComponent<Cannonball>();
			cannonBall.originator = GetComponent<Cloudship>();
        }
    }
}
