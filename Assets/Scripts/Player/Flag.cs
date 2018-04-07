using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    IWindMaker windMaker;

    void Start()
    {
        windMaker = GameManager.Instance.WindMaker;
    }

    void FixedUpdate()
    {
        var cycloneForce = windMaker.GetCycloneForce(transform.position) * Time.deltaTime;
                if (cycloneForce.magnitude == 0)
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            return;
        }

        var windDirection = Quaternion.LookRotation(-cycloneForce);

        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            windDirection,
            Time.deltaTime * 1.5f);
    }
}