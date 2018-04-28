using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour {

	static string BuilderLocationTag = "BuilderLocation";
	static string BuildingTag = "Building";

	public Material HighlightedMaterial;
	public GameObject BuildingSpaceIndicatorPrefab;

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
			var position =  map.Key.ToWorld() + mapOffset;
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
		if (selected.tag == BuildingTag)
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
		else if (selected.tag == BuildingTag)
		{
			selectedBuilding = selected;
			var building = selected.GetComponent<Building>();
			ClearBuildingIfExists(building);
			var renderer = selectedBuilding.GetComponent<Renderer>();
			originalMaterial = renderer.sharedMaterial;
			renderer.sharedMaterial = HighlightedMaterial;
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
			if (selectedBuilding == null)
			{
				if (GetEmptyLocation(location))
				{
					selectedBuilding = Instantiate(
						buildMenu.SelectedBuilding, 
						Vector3.zero, 
						Quaternion.identity, 
						this.transform);

					selectedBuilding.tag = BuildingTag;
					selectedBuilding.transform.localScale = Vector3.one;
					selectedBuilding.transform.localRotation = Quaternion.identity;
					var renderer = selectedBuilding.GetComponentInChildren<Renderer>();
					originalMaterial = renderer.sharedMaterial;
					renderer.sharedMaterial = HighlightedMaterial;
				}
			}
			if (selectedBuilding != null) 
			{
				if (GetEmptyLocation(location))
				{
					var building = selectedBuilding.GetComponent<Building>();	
					building.GridSpaceLocation = location.GridSpaceLocation;
					selectedBuilding.transform.localPosition = GridSpaceToLocalSpace(building);
				}
			}
		}
	}

	Vector3 GridSpaceToLocalSpace(Building building)
	{
		var grid = building.GridSpaceLocation;

		var buildingSizeOffset = Vector3.zero;
		if (building.Size.IsLargerThanUnit)
		{
			buildingSizeOffset = new Vector3(
				((building.Size.WidthX) / 2) * (gridSize / 2),
				0,
				((building.Size.LengthZ) / 2) * (gridSize / 2));
		}

		var position = new Vector3((grid.X * gridSize),0, (grid.Z * gridSize))
			+ buildingSizeOffset
			+ mapOffset;

		return position;
	}

	void MouseUp()
	{
		if (selectedBuilding != null)
		{
			var building = selectedBuilding.GetComponent<Building>();
			var renderer = selectedBuilding.GetComponentInChildren<Renderer>();
			ClearBuildingIfExists(building);
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
		for(int width = 0; width < building.Size.WidthX; width++)
		{
			for(int length =0; length < building.Size.LengthZ; length++)	
			{
				var alsogridLocation = new Grid{
					X = building.GridSpaceLocation.X + width,
					Z = building.GridSpaceLocation.Z + length
				};

				buildingMap[alsogridLocation] = building;
				selectedBuilding = null;
			}
		}
	}

	void ClearBuildingIfExists(Building building)
	{
		for(int width = 0; width < building.Size.WidthX; width++)
		{
			for(int length =0; length < building.Size.LengthZ; length++)	
			{
				var alsogridLocation = new Grid{
					X = building.GridSpaceLocation.X + width,
					Z = building.GridSpaceLocation.Z + length
				};
				buildingMap[alsogridLocation] = null;
			}
		}
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
	
	IHaveGridSpace NearestEmptySpace(IHaveGridSpace desiredLocation, BuildingSize size)
	{
		
		return desiredLocation;
	}

	bool WillItFit(Grid gridLocation, BuildingSize size)
	{
		for(int width = 0; width < size.WidthX; width++)
		{
			for(int length =0; length < size.LengthZ; length++)	
			{
				var testGridLocation = new Grid{
					X = gridLocation.X + width,
					Z = gridLocation.Z + length
				};
				
				if (!CanIBuildThere(testGridLocation))
				{
					return false;
				}
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
