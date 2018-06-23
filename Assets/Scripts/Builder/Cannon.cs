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
    string fireButton = "Fire1";
    float lastTimeFired;

    float BarrelElevation = 65f;
    IAmAShip shooter;
    float fuzzyShotForce = 0.005f;
    float MaxRange = 680;
    float MinRange = 180;

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

        var target = GetMyTarget();

        if (Time.time >= lastTimeFired)
        {
            if (Input.GetButtonUp(fireButton) || shooter.ShootFullAuto)
            {
                Shoot(target);
                lastTimeFired = Time.time + TimeBetweenShotsInSeconds;
            }
        }

        
        SetElevation(target);
    }

    void Shoot(GameObject target)
    {
        if (target == null || 
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

        var forceRandomiser = Random.Range(1 - fuzzyShotForce, 1+fuzzyShotForce);
        var ballForce = ShootingTip.rotation * Vector3.up * ShotForce * forceRandomiser;
        cannonBall.Stats.Force = ShotForce * forceRandomiser;
        cannonBall.Stats.StartElevation = ShootingTip.parent.eulerAngles.x;
        ball.AddForce(ballForce, ForceMode.Impulse);
        
    }

    GameObject GetMyTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(shooter.MyEnemyTagIs);
        
        foreach(var target in targets)
        {
            var targetDirection = target.transform.position - ShootingTip.position;
            float angle = Vector3.Angle(targetDirection, Swivel.forward);
            bool isInArc = Mathf.Abs(angle) < MaxFiringAngle;
            bool isInRange = (targetDirection.magnitude < MaxRange || shooter.IAmAPlayer);

            if (isInArc && isInRange)
            {
                return target;
            }
        }
        return null;
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

    void SetElevation(GameObject target)
    {
        if (target == null)
        {
            return;
        }
        var range = (target.transform.position - ShootingTip.position).magnitude;
        BarrelElevation = Maths.Rescale(90, 65, MinRange, MaxRange, range);
        Barrel.localRotation = Quaternion.Euler(BarrelElevation, 0, 0);
    }
}
