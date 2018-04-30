using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Rigidbody cannonball;
    public Transform Swivel;
    public Transform Barrel;
    public float TimeBetweenShotsInSeconds = 0.5f;
    public float ShotForce = 100f;

    [Range(0.2f, 0.8f)]
    public float SwivelSpeed = 0.4f;

    private string fireButton = "Fire1";
    private float lastTimeFired;

    private Building building;
    private Cloudship cloudship;

    void Start()
    {
        building = GetComponent<Building>();
        cloudship = GameManager.Instance.PlayerCloudship;
    }

    void Update()
    {
        // TODO ROLA refactor when Building creation is changed.
        if (building.GridSpaceLocation == null || cloudship.Mode == Cloudship.Modes.Build)
        {
            return;
        }

        if (Time.time >= lastTimeFired && Input.GetButtonUp(fireButton))
        {
            Shoot();
            lastTimeFired = Time.time + TimeBetweenShotsInSeconds;
        }

        TurnTowardEnemy();
    }

    void TurnTowardEnemy()
    {
        var lookVector = NearestTarget() - Swivel.position;
        var lookingAtEnemy = Quaternion.LookRotation(lookVector, transform.up);
        Swivel.rotation = Quaternion.Lerp(Swivel.rotation, lookingAtEnemy, Time.deltaTime * SwivelSpeed);

        Barrel.localRotation = Quaternion.Euler(45f, 0, 0);
    }

    void Shoot()
    {
        Rigidbody ball = Instantiate(
            cannonball, 
            Barrel.position, 
            Quaternion.identity,
            transform) as Rigidbody;

        var cannonBall = ball.GetComponent<Cannonball>();
        cannonBall.originator = cloudship;

        var ballForce = Barrel.rotation * Vector3.up * ShotForce;
        Debug.DrawRay(Barrel.position, ballForce * 100f, Color.green);

        ball.AddForce(ballForce, ForceMode.Impulse);
        
    }

    Vector3 NearestTarget()
    {
        GameObject[] targets;
        
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        
        if (targets.Length > 0)
        {
            return targets[0].transform.position;
        }
        return Barrel.forward;
    }
}
