using System;
using System.Collections.Generic;
using UnityEngine;

public class PressureBalls : MonoBehaviour
{
    public GameObject pressureBall;
    public float Magnitude;

    Cloudship playerCloudship;
    IWindMaker windMaker;
    int indicatorGridWith = 1;
    Dictionary<Vector3, GameObject> balls = new Dictionary<Vector3, GameObject>();
    int distanceFromPlayer = 15;
    
    public void Start() 
    {
        playerCloudship = GameManager.Instance.PlayerCloudship;
        windMaker = GameManager.Instance.WindMaker;
    }

    public void Update()
    {
        var centre = playerCloudship.Position;

        int startX = (int)centre.x - distanceFromPlayer;
        int endX = (int)centre.x + distanceFromPlayer;
        int startZ = (int)centre.z - distanceFromPlayer;
        int endZ = (int)centre.z + distanceFromPlayer;

        for (int x = startX; x < endX; x++)
        {
            for (int z = startZ; z < endZ; z++)
            {
                var position = new Vector3(x, 0, z);
                if (CanIMakeANewBall(position))
                {
                    var Magnitude = windMaker.WindMagnitudeAt(position);

                    if (!AlreadyBallThere(position))
                    {
                        balls[position] = Instantiate(pressureBall, position, Quaternion.identity);
                        SetColour(balls[position], Magnitude);
                    }
                }
            }
        }
    }
    
    private void SetColour(GameObject indicator, float magnitude)
    {
        Renderer rend = indicator.GetComponent<Renderer>();
        rend.material.color = new Color(magnitude, magnitude, magnitude);
   }
    private bool CanIMakeANewBall(Vector3 position)
    {
        return (position.x % indicatorGridWith == 0) && 
        (position.z % indicatorGridWith == 0);
    }

    private bool AlreadyBallThere(Vector3 position)
    {
        return balls.ContainsKey(position);
    }
}