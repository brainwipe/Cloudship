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
            var shooter = GetComponent<Cloudship>();
            var lookVector = (NearestTarget(shooter) - shooter.transform.position).normalized;
            
			Rigidbody ball = Instantiate(
                cannonball, 
                cannon.position, 
                Quaternion.LookRotation(lookVector)) as Rigidbody;
            var cannonBall = ball.GetComponent<Cannonball>();
			cannonBall.originator = shooter;
            ball.AddForce(lookVector * 5, ForceMode.Impulse);
        }
    }

    private Vector3 NearestTarget(Cloudship shooter)
    {
        var target = GameObject.FindGameObjectsWithTag("Enemy");
        return target[0].transform.position;
    }
}
