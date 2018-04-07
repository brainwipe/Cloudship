using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour {

    Cloudship player;

    float[] menuPositions = {
        0f, 60f, 120f, 180f, 240f, 300f
    };

    public int selectionIndex = 0;

    float wheelSpeed = 4f;

    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
    }

    void Update()
    {
        if (player.Mode == Cloudship.Modes.Build)
        {
            UpdateSelection(Input.GetButtonUp("Throttle Up"), Input.GetButtonUp("Throttle Down"));
            UpdateMenuPosition();
        }
    }

    void UpdateMenuPosition()
	{
        var target = Quaternion.Euler(menuPositions[selectionIndex], 0, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target, Time.deltaTime * wheelSpeed);
	}

    void UpdateSelection(bool up, bool down)
    {
        if (up)
		{
			selectionIndex++;
		}
		else if (down)
		{
			selectionIndex--;
		}

		if (selectionIndex < 0)
		{
			selectionIndex = menuPositions.Length - 1;
		}

		if (selectionIndex > menuPositions.Length -1)
		{
			selectionIndex = 0;
		}
    }

}
