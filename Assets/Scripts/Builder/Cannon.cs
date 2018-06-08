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
    public float BarrelElevation = 65f;

    private string fireButton = "Fire1";
    private float lastTimeFired;

    private Cloudship cloudship;
    private float fuzzyShotForce = 0.2f;

    void Start()
    {
        cloudship = GameManager.Instance.PlayerCloudship;
        Barrel.localRotation = Quaternion.Euler(BarrelElevation, 0, 0);
    }

    void Update()
    {
        if (cloudship.Mode == Cloudship.Modes.Build)
        {
            return;
        }

        if (Time.time >= lastTimeFired && Input.GetButtonUp(fireButton))
        {
            Shoot();
            lastTimeFired = Time.time + TimeBetweenShotsInSeconds;
        }
    }

    void Shoot()
    {
        if (!AmIClearToShoot(ShootingTip.position, ShootingTip.rotation))
        {
            return;
        }

        Rigidbody ball = Instantiate(
            cannonball, 
            ShootingTip.position, 
            Quaternion.identity,
            transform) as Rigidbody;

        var cannonBall = ball.GetComponent<Cannonball>();
        cannonBall.owner = cloudship;

        var forceRandomiser = Random.Range(1 - fuzzyShotForce, 1+fuzzyShotForce);
        var ballForce = ShootingTip.rotation * Vector3.up * ShotForce * forceRandomiser;

        ball.AddForce(ballForce, ForceMode.Impulse);
        
    }

    bool AmIClearToShoot(Vector3 shootingTipPosition, Quaternion shootingTipRotation)
    {
        var shootingTipDirection = shootingTipRotation * Vector3.one;
        RaycastHit hit;
		int layerMask = 1 << 10;
		if (Physics.Raycast(shootingTipPosition, shootingTipDirection,  out hit, Mathf.Infinity, layerMask))
		{
			if (hit.transform.tag == "Building")
			{
                return false;
            }
        }
        return true;
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
