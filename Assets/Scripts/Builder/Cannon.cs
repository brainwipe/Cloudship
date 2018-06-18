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

    string fireButton = "Fire1";
    float lastTimeFired;

    IAmAShip parent;
    float fuzzyShotForce = 0.2f;

    void Start()
    {
        Barrel.localRotation = Quaternion.Euler(BarrelElevation, 0, 0);
        parent = transform.GetComponentInParent<IAmAShip>();  
    }

    void Update()
    {
        if (parent == null || !parent.CanShoot)
        {
            return;
        }

        if (Time.time >= lastTimeFired)
        {
            if (Input.GetButtonUp(fireButton) || parent.ShootFullAuto)
            {
                Shoot();
                lastTimeFired = Time.time + TimeBetweenShotsInSeconds;
            }
        }
    }

    void Shoot()
    {
        if (!IsThereATargetInMyArc() || 
            !AmIClearToShoot(ShootingTip.position, ShootingTip.rotation))
        {
            return;
        }

        Rigidbody ball = Instantiate(
            cannonball, 
            ShootingTip.position, 
            Quaternion.identity,
            transform) as Rigidbody;

        var cannonBall = ball.GetComponent<Cannonball>();
        cannonBall.owner = parent;

        var forceRandomiser = Random.Range(1 - fuzzyShotForce, 1+fuzzyShotForce);
        var ballForce = ShootingTip.rotation * Vector3.up * ShotForce * forceRandomiser;

        ball.AddForce(ballForce, ForceMode.Impulse);
        
    }

    bool IsThereATargetInMyArc()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(parent.MyEnemyTagIs);
        if (targets.Length > 0)
        {
            foreach(var target in targets)
            {
                var targetDirection = target.transform.position - ShootingTip.position;
                float angle = Vector3.Angle(targetDirection, Swivel.forward);
                
                if (Mathf.Abs(angle) < 15f)
                {
                    return true;
                }
            }
        }
        return false;
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
}
