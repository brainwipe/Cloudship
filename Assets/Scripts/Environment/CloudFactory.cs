using System;
using System.Collections.Generic;
using UnityEngine;

public class CloudFactory : MonoBehaviour
{
    public GameObject cloudPrefab;

    public GameObject cloud;

    public IWindMaker windMaker;

    float windEffect = 0.15f;

    public void Start()
    {
        cloud = Instantiate(cloudPrefab, Vector3.one, Quaternion.identity);
        windMaker = GetComponent<IWindMaker>();
    }

    public void FixedUpdate()
    {
        var cycloneForce = windMaker.GetCycloneForce(cloud.transform.position) * Time.deltaTime;
        cloud.transform.position += cycloneForce * windEffect;
    }
}