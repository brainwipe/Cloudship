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
        shooter = GetComponent<Enemy>();
    }

    void Update()
    {
        if (Time.time >= lastTimeFired)
        {
            var nearestTargetPosition = NearestTargetPosition();
            Shoot(nearestTargetPosition);
            lastTimeFired = Time.time + TimeBetweenShotsInSeconds;
        }
    }

    void Shoot(Vector3 nearestTargetPosition)
    {
        var lookVector = (nearestTargetPosition - transform.position).normalized;
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

    Vector3 NearestTargetPosition()
    {
        GameObject[] targets;
        targets = GameObject.FindGameObjectsWithTag("Player");

        if (targets.Length > 0)
        {

            GameObject nearest = null;
            float lowestSqrMagnitude = float.MaxValue;
            foreach(var target in targets)
            {
                var range = (target.transform.position - shooter.Position).sqrMagnitude;
                if (range < lowestSqrMagnitude)
                {
                    lowestSqrMagnitude = range;
                    nearest = target;
                }
            }
       
            return nearest.transform.position;
        }
        return transform.forward;
    }
}
