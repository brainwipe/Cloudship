using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody cannonball;
    public Transform cannon;
    public float TimeBetweenShotsInSeconds = 0.5f;
    public float ShotForce = 100f;
    private IAmAShip shooter;

    private string fireButton = "Fire1";
    private float lastTimeFired;

    void Start()
    {
        shooter = GetComponent<Cloudship>();
        if (shooter == null)
        {
            shooter = GetComponent<Enemy>();
        }
    }

    void Update()
    {
        if (shooter is Cloudship)
        {
            if (Time.time >= lastTimeFired && Input.GetButtonUp(fireButton))
            {
                Shoot();
                lastTimeFired = Time.time + TimeBetweenShotsInSeconds;
            }
        }
        else
        {
            if (Time.time >= lastTimeFired)
            {
                Shoot();
                lastTimeFired = Time.time + TimeBetweenShotsInSeconds;
            }
        }
    }

    void Shoot()
    {
        var lookVector = (NearestTarget() - transform.position).normalized;
        var shootVector = Vector3.RotateTowards(lookVector, Vector3.up, 0.3f, 10f);
        
        Rigidbody ball = Instantiate(
            cannonball, 
            cannon.position, 
            Quaternion.LookRotation(shootVector),
            transform) as Rigidbody;

        var cannonBall = ball.GetComponent<Cannonball>();
        cannonBall.owner = shooter;
        ball.AddForce(shootVector * ShotForce, ForceMode.Impulse);
    }

    private Vector3 NearestTarget()
    {
        GameObject[] targets;
        if (shooter is Cloudship)
        {
            targets = GameObject.FindGameObjectsWithTag("Enemy");
        }
        else
        {
            targets = GameObject.FindGameObjectsWithTag("Player");
        }

        if (targets.Length > 0)
        {
            return targets[0].transform.position;
        }
        return transform.forward;
    }
}
