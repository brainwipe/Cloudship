using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Rigidbody cannonball;
    public Transform Swivel;
    public Transform Barrel;
    public Transform ShootingTip;
    public ParticleSystem[] Puffs;

    public float TimeBetweenShotsInSeconds;
    public float ShotForce;
    public float MaxFiringAngle;
    string fireButton = "Fire1";
    float lastTimeFired;

    IAmAShip shooter;
    float fuzzyShotForce = 0.01f;
    float MaxRange => rangeMaps.Max(x => x.Range);
    Vector3 forecastPosition;

    RangeMap[] rangeMaps = new [] 
    {
        new RangeMap { Time = 0, Range = 93, Elevation = 90 },
        new RangeMap { Time = 3, Range = 221, Elevation = 85 },
        new RangeMap { Time = 4, Range = 370, Elevation = 80 },
        new RangeMap { Time = 5, Range = 509, Elevation = 75 },
        new RangeMap { Time = 7, Range = 651, Elevation = 70 },
        new RangeMap { Time = 8, Range = 780, Elevation = 65 },
    };

    void Start()
    {
        Barrel.localRotation = Quaternion.Euler(rangeMaps.Min(x => x.Elevation), 0, 0);
        shooter = transform.GetComponentInParent<IAmAShip>();  
    }

    void Update()
    {
        if (shooter == null || !shooter.CanShoot)
        {
            return;
        }

        var target = GetMyTarget();
        SetElevation(target);

        if (Time.time >= lastTimeFired)
        {
            if (Input.GetKey(KeyCode.Space) || shooter.ShootFullAuto)
            {
                Shoot(target);
                lastTimeFired = Time.time + TimeBetweenShotsInSeconds;
            }
        }
    }

    void Shoot(IAmATarget target)
    {
        if ((target == null && !shooter.FireAtWill) || 
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
        var ballForce = (ShootingTip.rotation * Vector3.up * ShotForce * forceRandomiser) + shooter.Velocity;
        
        ball.AddForce(ballForce, ForceMode.Impulse);

        cannonBall.Stats.Force = ShotForce * forceRandomiser;
        cannonBall.Stats.StartElevation = ShootingTip.parent.eulerAngles.x;
        
        foreach(var puff in Puffs)
        {
            puff.Play();
        }
    }

    IAmATarget GetMyTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(shooter.MyEnemyTagIs);
        
        IAmATarget nearestTarget = null;
        float nearestRange = float.MaxValue;

        foreach(var target in targets)
        {
            var targetDirection = target.transform.position - ShootingTip.position;
            float range = targetDirection.magnitude;

            if (range > nearestRange ||
                (range > MaxRange && !shooter.IAmAPlayer))
            {
                continue;
            }
            
            float angle = Vector3.Angle(targetDirection, Swivel.forward);
            if (Mathf.Abs(angle) > MaxFiringAngle)
            {
                continue;
            }

            var targetComponent = target.GetComponentInParent<IAmATarget>();
            if (targetComponent == null || targetComponent.IsDead)
            {
                continue;
            }
            
            nearestRange = range;
            nearestTarget = targetComponent;
        }
        return nearestTarget;
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

    void SetElevation(IAmATarget target)
    {
        float forecastElevation = rangeMaps.Min(x => x.Elevation);
        if (target != null)
        {
            float currentRange = (target.Position - ShootingTip.position).magnitude;
            var timeToCurrentRange = TimeFromRange(currentRange);
            forecastPosition = target.Position + (target.Velocity * timeToCurrentRange);
            var forecastRange = (forecastPosition - ShootingTip.position).magnitude;
            forecastElevation = ElevationFromRange(forecastRange);
        }
         Barrel.localRotation = Quaternion.Euler(forecastElevation, 0, 0);
    }

/*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (shooter != null && shooter.IAmAPlayer)
        {
            Gizmos.color = Color.green;
        }

        if (forecastPosition != Vector3.zero)
        {
            Gizmos.DrawWireSphere(shooter.Position, MaxRange);
            Gizmos.DrawWireSphere(forecastPosition, 5);
        }
    }
*/

    float ElevationFromRange(float range)
    {
        for(int i = 0; i < rangeMaps.Length; i++)
        {
            if (range > rangeMaps[i].Range)
            {
                continue;
            }

            if (i == 0)
            {
                return rangeMaps.Max(x => x.Elevation);
            }

            return Maths.Rescale(rangeMaps[i-1].Elevation, rangeMaps[i].Elevation, rangeMaps[i-1].Range, rangeMaps[i].Range, range);
        }

        return rangeMaps.Min(x => x.Elevation);
    }

    float TimeFromRange(float range)
    {
        for(int i = 0; i < rangeMaps.Length; i++)
        {
            if (range > rangeMaps[i].Range)
            {
                continue;
            }

            if (i == 0)
            {
                return rangeMaps[0].Time;
            }

            return Maths.Rescale(rangeMaps[i-1].Time, rangeMaps[i].Time, rangeMaps[i-1].Range, rangeMaps[i].Range, range);
        }

        return rangeMaps.Last().Time;
    }

    struct RangeMap
    {
        public float Range;
        public float Time;
        public float Elevation;
    }
}
