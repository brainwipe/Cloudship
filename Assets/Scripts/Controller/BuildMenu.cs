using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour {

    Cloudship player;

    MenuPosition[] menuPositions = {
        new MenuPosition(0f),
        new MenuPosition(60f),
        new MenuPosition(120f),
        new MenuPosition(180f),
        new MenuPosition(240f),
        new MenuPosition(300f),
    };

    int selectionIndex = 0;
    int buildingIndex = 0;
    float wheelSpeed = 4f;
    GameObject[] BuildingPrefabs;
    
    
    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
        BuildingPrefabs = BuildingLoader.Load();

        FillWheel();
    }

    void Update()
    {
        if (player.Mode == Cloudship.Modes.Build)
        {
            MenuUpdate(
                Input.GetButtonUp("Throttle Up"), 
                Input.GetButtonUp("Throttle Down"));
            UpdateMenuPosition();
        }
    }

    void CreateBuildingAt(int buildingIndex, int menuIndex)
    {
        var building = Instantiate(BuildingPrefabs[buildingIndex],  this.transform.position, Quaternion.identity, this.transform);
        building.layer = gameObject.layer;
        building.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
        building.transform.localPosition = new Vector3(0,0,0.27f);
        building.transform.RotateAround(
            transform.position, 
            -transform.right, 
            menuPositions[menuIndex].Angle);
        menuPositions[menuIndex].Building = building;
    }

    void FillWheel()
    {
       CreateBuildingAt(0,0);
       CreateBuildingAt(1,1);
       CreateBuildingAt(2,2);
       CreateBuildingAt(BuildingPrefabs.Length - 1, menuPositions.Length - 1);
       CreateBuildingAt(BuildingPrefabs.Length - 2, menuPositions.Length - 2);
    }
    
    // CLOCKWISE = UP = POSTIVE!
    void MenuUpdate(bool clockwise, bool anticlockwise)
    {
        if (!clockwise && !anticlockwise)
        {
            return;
        }

        int directionMultiplier = 1;
        if (anticlockwise)
        {
            directionMultiplier = -1;
        }

        selectionIndex = WrapAround(menuPositions.Length, selectionIndex + (1 * directionMultiplier));
        buildingIndex = WrapAround(BuildingPrefabs.Length, buildingIndex + (1 * directionMultiplier));

        var forwardMenuIndex = WrapAround(menuPositions.Length, selectionIndex + (2 * directionMultiplier));
        var forwardBuildingIndex = WrapAround(BuildingPrefabs.Length, buildingIndex + (2 * directionMultiplier));
        Destroy(menuPositions[forwardMenuIndex].Building);
        CreateBuildingAt(forwardBuildingIndex, forwardMenuIndex);
    }

    void UpdateMenuPosition()
	{
        var target = Quaternion.Euler(menuPositions[selectionIndex].Angle, 0, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target, Time.deltaTime * wheelSpeed);
	}

    int WrapAround(int length, int index)
    {
        if (index > length - 1)
        {
            return index - length;
        }
        else if (index < 0)
        {
            return length + index;
        }
        return index;
    }

    private class MenuPosition
    {
        float angle;
        public MenuPosition(float angle)
        {
            this.angle = angle;
        }

        public GameObject Building 
        {
            get;set;
        }

        public float Angle{
            get{
                return angle;
            }
        }
    }
}
