using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody cannonball;
    public Transform cannon;
    private ITakeDamage shooter;

    private string fireButton = "Fire1";
    private bool fired;
    public float timeBetweenShotsInSeconds = 0.5f;
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
        if (Time.time >= lastTimeFired && Input.GetButtonUp(fireButton))
        {
            Shoot();
            lastTimeFired = Time.time + timeBetweenShotsInSeconds;
        }
    }

    void Shoot()
    {
        var lookVector = (NearestTarget() - transform.position).normalized;
        var shootVector = Vector3.RotateTowards(lookVector, Vector3.up, 0.785398f, 10f);
        
        Debug.DrawRay(transform.position, shootVector, Color.green);

        Rigidbody ball = Instantiate(
            cannonball, 
            cannon.position, 
            Quaternion.LookRotation(shootVector)) as Rigidbody;

        var cannonBall = ball.GetComponent<Cannonball>();
        cannonBall.originator = shooter;
        ball.AddForce(shootVector * 5, ForceMode.Impulse);
    }

    private Vector3 NearestTarget()
    {
        var target = GameObject.FindGameObjectsWithTag("Enemy");
        if (target.Length > 0)
        {
            return target[0].transform.position;
        }
        return transform.forward;
    }
}
