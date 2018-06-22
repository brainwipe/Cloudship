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
    public float MaxFiringAngle = 25f;
    public float MaxRange = 600;
    string fireButton = "Fire1";
    float lastTimeFired;

    float BarrelElevation = 65f;
    IAmAShip shooter;
    float fuzzyShotForce = 0.1f;

    void Start()
    {
        Barrel.localRotation = Quaternion.Euler(BarrelElevation, 0, 0);
        shooter = transform.GetComponentInParent<IAmAShip>();  
    }

    void Update()
    {
        if (shooter == null || !shooter.CanShoot)
        {
            return;
        }

        if (Time.time >= lastTimeFired)
        {
            if (Input.GetButtonUp(fireButton) || shooter.ShootFullAuto)
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
            GameManager.Instance.Cannonballs) as Rigidbody;

        var cannonBall = ball.GetComponent<Cannonball>();
        cannonBall.owner = shooter;

        var forceRandomiser = 1;// Random.Range(1 - fuzzyShotForce, 1+fuzzyShotForce);
        var ballForce = ShootingTip.rotation * Vector3.up * ShotForce * forceRandomiser;

        ball.AddForce(ballForce, ForceMode.Impulse);
        
    }

    bool IsThereATargetInMyArc()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(shooter.MyEnemyTagIs);
        if (targets.Length > 0)
        {
            foreach(var target in targets)
            {
                var targetDirection = target.transform.position - ShootingTip.position;
                float angle = Vector3.Angle(targetDirection, Swivel.forward);
                bool isInArc = Mathf.Abs(angle) < MaxFiringAngle;
                bool isInRange = targetDirection.magnitude < MaxRange;

                if (isInArc && isInRange)
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
