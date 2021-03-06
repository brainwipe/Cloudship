using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Windicators : MonoBehaviour
{
    public GameObject weatherIndicator;
    
    Cloudship playerCloudship;
    IWindMaker windMaker;
    int distanceFromPlayer = 2000;
    float sqrDisplacementBetweenUpdates = 1000;
    private Vector3 positionOfLastUpdate;
    bool hasInitialSetBeenMade;

    int indicatorGridWith = 200;
    Dictionary<Vector3, GameObject> indicators = new Dictionary<Vector3, GameObject>();
    
    void Start()
    {
        hasInitialSetBeenMade = false;
        playerCloudship = GameManager.Instance.PlayerCloudship;
        windMaker = GameManager.Instance.WindMaker;
    }

    public void Update()
    {
        var centre = playerCloudship.Position;

        if (ShouldIMakeMoreWindicators(centre))
		{
		    int startX = (int)centre.x - distanceFromPlayer;
            int endX = (int)centre.x + distanceFromPlayer;
            int startZ = (int)centre.z - distanceFromPlayer;
            int endZ = (int)centre.z + distanceFromPlayer;

            for (int x = startX; x < endX; x++)
            {
                for (int z = startZ; z < endZ; z++)
                {
                    var position = new Vector3(x, 0, z);
                    if (CanIMakeANewIndicator(position))
                    {
                        var force = windMaker.GetCycloneForce(position);

                        var rotation = Quaternion.FromToRotation(Vector3.back, force);
                        var magnitude = force.magnitude * 0.3f;
                        var newScale = new Vector3(magnitude, 0.5f, magnitude);

                        if (!AlreadyIndicatorThere(position))
                        {
                            indicators[position] = Instantiate(weatherIndicator, position, rotation);
                            indicators[position].transform.localScale = newScale;
                            indicators[position].transform.parent = this.transform;
                            SetColour(indicators[position], rotation);
                        }
                    }
                }
            }
            positionOfLastUpdate = centre;
            hasInitialSetBeenMade = true;
        }
    }

    private void SetColour(GameObject indicator, Quaternion rotation)
    {
        var vectorAxis = Vector3.up;
        float angle;
        rotation.ToAngleAxis(out angle, out vectorAxis);
        angle = Mathf.Abs(angle);
        float h = angle / 360f;
        var color = Color.HSVToRGB(h, 1f, 1f);

        Renderer rend = indicator.GetComponent<Renderer>();
        rend.material.color = color;
    }

    private bool AlreadyIndicatorThere(Vector3 position)
    {
        return indicators.ContainsKey(position);
    }

    private bool CanIMakeANewIndicator(Vector3 position)
    {
        return (position.x % indicatorGridWith == 0) && 
        (position.z % indicatorGridWith == 0);
    }

    private bool ShouldIMakeMoreWindicators(Vector3 playerCloudshipPosition)
	{
        if (hasInitialSetBeenMade == false)
        {
            return true;
        }
		return 
        ((positionOfLastUpdate - playerCloudshipPosition).sqrMagnitude > 
			sqrDisplacementBetweenUpdates);
	}

}
