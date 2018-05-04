using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Rigidbody cannonball;
    public Transform Swivel;
    public Transform Barrel;
    public Transform ShootingTip;
    public float TimeBetweenShotsInSeconds = 0.5f;
    public float ShotForce = 1000f;

    [Range(0.2f, 0.8f)]
    public float SwivelSpeed = 0.4f;

    private string fireButton = "Fire1";
    private float lastTimeFired;

    private Cloudship cloudship;
    private float fuzzyShotForce = 0.2f;

    void Start()
    {
        cloudship = GameManager.Instance.PlayerCloudship;
    }

    void Update()
    {
        // TODO ROLA refactor when Building creation is changed.
        if (cloudship.Mode == Cloudship.Modes.Build)
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
        var difference = NearestTarget() - Swivel.position;
        var lookVector = new Vector3(difference.x, Swivel.position.y, difference.z);
        var lookingAtEnemy = Quaternion.LookRotation(lookVector, transform.up);
        Swivel.rotation = Quaternion.Lerp(Swivel.rotation, lookingAtEnemy, Time.deltaTime * SwivelSpeed);

        Barrel.localRotation = Quaternion.Euler(65f, 0, 0);
    }

    void Shoot()
    {
        Rigidbody ball = Instantiate(
            cannonball, 
            ShootingTip.position, 
            Quaternion.identity,
            transform) as Rigidbody;

        var cannonBall = ball.GetComponent<Cannonball>();
        cannonBall.originator = cloudship;

        var forceRandomiser = Random.Range(1 - fuzzyShotForce, 1+fuzzyShotForce);
        var ballForce = ShootingTip.rotation * Vector3.up * ShotForce * forceRandomiser;

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
