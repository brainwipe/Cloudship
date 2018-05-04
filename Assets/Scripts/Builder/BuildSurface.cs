using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour {

	static string BuilderLocationTag = "BuilderLocation";

	public GameObject BuildingSpaceIndicatorPrefab;

	Building selectedBuilding;
	BuildMenu buildMenu;

	Dictionary<Grid, IAmBuilding> buildingMap = new Dictionary<Grid, IAmBuilding>
	{
		{ Grid.From(-1,-4), null },
		{ Grid.From(0,-4), null },
		{ Grid.From(-1,-3), null },
		{ Grid.From(0,-3), null },
		{ Grid.From(-1,-2), null },
		{ Grid.From(0,-2), null },
		{ Grid.From(-2,-1), null },
		{ Grid.From(-1,-1), null },
		{ Grid.From(-0,-1), null },
		{ Grid.From(1,-1), null },
		{ Grid.From(-2,0), null },
		{ Grid.From(-1,0), null },
		{ Grid.From(0,0), null },
		{ Grid.From(1,0), null },
		{ Grid.From(-1,1), null },
		{ Grid.From(0,1), null },
		{ Grid.From(-1,2), null },
		{ Grid.From(0,2), null },
		{ Grid.From(-1,3), null },
		{ Grid.From(0,3), null },
	};

	void Awake()
	{
		CreateBuildingLocations();
		if (buildMenu == null)
		{
			buildMenu = FindObjectOfType<BuildMenu>();
		}
	}

	void OnDisable()
	{
		HideBuildingLocations();
	}

	void OnEnable()
	{
		ShowBuildingLocations();
	}
	
	void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			MouseDown();
		}

		if (Input.GetMouseButton(0))
		{
			MouseMove();
			if (Input.GetKeyUp(KeyCode.Delete))
			{
				Delete();
			}
		}
	
		if(Input.GetMouseButtonUp(0))
		{
			MouseUp();
		}
	}

	void CreateBuildingLocations()
	{
		foreach(var map in buildingMap)
		{
			var position =  map.Key.ToWorld();
			var location = Instantiate(BuildingSpaceIndicatorPrefab, position, Quaternion.identity, this.transform);
			location.tag = BuilderLocationTag;
			var buildLocation = location.GetComponent<BuildLocation>();
			buildLocation.GridSpaceLocation = map.Key;
			var renderer = location.GetComponent<Renderer>();
			renderer.enabled = false;
		}
	}

	void HideBuildingLocations()
	{
		var renderers = transform.FindObjectsWithTag(BuilderLocationTag).Select(x => x.GetComponent<Renderer>());
		foreach(var renderer in renderers)
		{
			renderer.enabled = false;
		}
	}

	void ShowBuildingLocations()
	{
		var renderers = transform.FindObjectsWithTag(BuilderLocationTag).Select(x => x.GetComponent<Renderer>());
		foreach(var renderer in renderers)
		{
			renderer.enabled = true;
		}
	}

	void Delete()
	{
		var selected = WhatIsUnderTheMousePointer();
		if (selected.tag == Building.BuildingTag)
		{
			var location = selected.GetComponent<Building>();
			ClearBuildingIfExists(location);
			Destroy(selected);
		}
	}

	void MouseDown()
	{
		var selected = WhatIsUnderTheMousePointer();
		if (selected == null)
		{
			return;
		}
		else if (selected.tag == Building.BuildingTag)
		{
			selectedBuilding = selected.GetComponent<Building>();
			ClearBuildingIfExists(selectedBuilding);
			selectedBuilding.ToggleHighlight();
		}
	}

	void MouseMove()
	{
		var selected = FindLocationUnderMouse();

		if (selected == null)
		{
			return;
		}

		if (selected.tag == BuilderLocationTag)
		{
			var location = selected.GetComponent<BuildLocation>();
			if (GetEmptyLocation(location))
			{
				if (selectedBuilding == null)
				{
					selectedBuilding = buildMenu.SelectedBuilding.Clone(transform);
					selectedBuilding.ToggleHighlight();
				}
				else
				{
					selectedBuilding.GridSpaceLocation = location.GridSpaceLocation;
				}
			}
		}
	}

	void MouseUp()
	{
		if (selectedBuilding != null)
		{
			var building = selectedBuilding.GetComponent<Building>();
			ClearBuildingIfExists(building);
			selectedBuilding.ToggleHighlight();
			SaveBuilding(building);
		}
	}

	GameObject WhatIsUnderTheMousePointer()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			return hit.collider.gameObject;
		}
		return null;
	}

	GameObject FindLocationUnderMouse()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var layerMask = 1 << BuildingSpaceIndicatorPrefab.layer;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			return hit.collider.gameObject;
		}
		return null;
	}

	void SaveBuilding(Building building)
	{
		building.Locations.ForAll(b => buildingMap[b] = building);
		selectedBuilding = null;
	}

	void ClearBuildingIfExists(Building building)
	{
		building.Locations.ForAll(b => buildingMap[b] = null);
	}
	
	bool GetEmptyLocation(IHaveGridSpace location)
	{
		if (location == null)
		{
			return false;
		}

		var building = buildMenu.SelectedBuilding.GetComponent<Building>();
		if (buildingMap.ContainsKey(location.GridSpaceLocation) && WillItFit(location.GridSpaceLocation, building.Size))
		{
			return true;
		}
		
		return false;
	}
	
	bool WillItFit(Grid gridLocation, BuildingSize size)
	{
		foreach(var location in gridLocation.GetAllLocations(size))
		{
			if (!CanIBuildThere(location))
			{
				return false;
			}
		}
		
		return true;
	}

	bool CanIBuildThere(Grid gridLocation)
	{
		if (gridLocation == null)
		{
			return false;
		}

		if (!buildingMap.ContainsKey(gridLocation))
		{
			return false;
		}

		return buildingMap[gridLocation] == null;
	}
}
