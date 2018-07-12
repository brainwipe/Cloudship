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
    
    public Building SelectedBuilding 
    {
        get
        {
            return menuPositions[selectionIndex].Building;
        }
    }

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
            UpdateAffordability();
        }
    }

    void CreateBuildingAt(int buildingIndex, int menuIndex)
    {
        var position = menuPositions[menuIndex];
        
        var obj = Instantiate(BuildingPrefabs[buildingIndex], position.Anchor.transform.position, Quaternion.identity, position.Anchor.transform);
        var building = obj.GetComponent<Building>();
        building.SetupForMenu(gameObject.layer);
        position.Building = building;
    }

    void FillWheel()
    {
        foreach(var position in menuPositions)
        {
            position.Anchor = new GameObject();
            position.Anchor.transform.SetParent(this.transform, false);
            position.Anchor.transform.localPosition = new Vector3(0,0,0.27f);
            position.Anchor.transform.RotateAround(
                transform.position, 
                -transform.right, 
                position.Angle);
        }

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

        menuPositions[forwardMenuIndex].RemoveBuilding();
        CreateBuildingAt(forwardBuildingIndex, forwardMenuIndex);
    }

    void UpdateMenuPosition()
	{
        var target = Quaternion.Euler(menuPositions[selectionIndex].Angle, 0, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target, Time.deltaTime * wheelSpeed);
	}

    void UpdateAffordability()
    {
        foreach(var position in menuPositions)
        {
            if (position.Building != null)
            {
                position.Building.UpdateAffordability(player.TotalFlotsam);
            }
        }
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

        public GameObject Anchor
        {
            get;set;
        }

        public Building Building 
        {
            get;set;
        }

        public float Angle{
            get{
                return angle;
            }
        }

        public void RemoveBuilding()
        {
            if (Building != null)
            {
                Building.Remove();
            }
        }
    }
}
