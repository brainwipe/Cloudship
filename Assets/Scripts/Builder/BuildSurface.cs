using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour {

	static string BuilderLocationTag = "BuilderLocation";
	static string BuildingTag = "Building";

	public Material HighlightedMaterial;
	public GameObject BuildingSpaceIndicatorPrefab;
	public GameObject Buildings;

	Vector3 mapOffset = new Vector3(5f, 0f, 5f);
	float gridSize = 10f;

	GameObject selectedBuilding;
	BuildMenu buildMenu;
	public Material originalMaterial;

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
		{ Grid.From(-1,-1), null },
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
		}
	
		if(Input.GetMouseButtonUp(0))
		{
			MouseUp();
		}

		if (Input.GetKeyUp(KeyCode.Delete))
		{
			Delete();
		}
	}

	void CreateBuildingLocations()
	{
		foreach(var map in buildingMap)
		{
			var position =  map.Key.ToWorld() + mapOffset;
			var location = Instantiate(BuildingSpaceIndicatorPrefab, position, Quaternion.identity, this.transform);
			location.tag = BuilderLocationTag;
			location.layer = transform.parent.gameObject.layer;
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
		if (selected.tag == BuildingTag)
		{
			var location = selected.GetComponent<Building>();
			ClearBuilding(location);
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
		if (selected.tag == BuilderLocationTag)
		{
			var location = selected.GetComponent<BuildLocation>();
			if (GetEmptyLocation(location))
			{
				selectedBuilding = Instantiate(
					buildMenu.SelectedBuilding, 
					GridSpaceToLocalSpace(location.GridSpaceLocation), 
					Quaternion.identity, 
					this.transform);
				selectedBuilding.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
				selectedBuilding.transform.position = GridSpaceToLocalSpace(location.GridSpaceLocation);
				selectedBuilding.tag = BuildingTag;
				var building = selectedBuilding.AddComponent<Building>();
				building.GridSpaceLocation = location.GridSpaceLocation;

				var renderer = selectedBuilding.GetComponent<Renderer>();
				originalMaterial = renderer.sharedMaterial;
			}
		}
		else if (selected.tag == BuildingTag)
		{
			selectedBuilding = selected;
			var building = selected.GetComponent<Building>();
			ClearBuilding(building);
			var renderer = selectedBuilding.GetComponent<Renderer>();
			originalMaterial = renderer.sharedMaterial;
			renderer.sharedMaterial = HighlightedMaterial;
		}
	}

	void MouseMove()
	{
		if (selectedBuilding != null)
		{
			var building = selectedBuilding.GetComponent<Building>();
			if (GetEmptyLocation(building))
			{
				selectedBuilding.transform.position = GridSpaceToLocalSpace(building.GridSpaceLocation);
			}
		}
	}

	Vector3 GridSpaceToLocalSpace(Grid grid)
	{
		return new Vector3(grid.X * gridSize, 0, grid.Y * gridSize) + mapOffset;
	}

	void MouseUp()
	{
		if (selectedBuilding != null)
		{
			var building = selectedBuilding.GetComponent<Building>();
			var renderer = selectedBuilding.GetComponent<Renderer>();
			renderer.sharedMaterial = originalMaterial;
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

	void SaveBuilding(Building building)
	{
		buildingMap[building.GridSpaceLocation] = building;
	}

	void ClearBuilding(Building building)
	{
		buildingMap[building.GridSpaceLocation] = null;
	}

	bool GetEmptyLocation(IHaveGridSpace location)
	{
		if (location == null)
		{
			return false;
		}

		if (buildingMap.ContainsKey(location.GridSpaceLocation) && buildingMap[location.GridSpaceLocation] == null)
		{
			return true;
		}
		
		return false;
	}
}
