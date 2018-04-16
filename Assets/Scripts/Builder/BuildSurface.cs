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

	GameObject selectedBuilding;
	public Material originalMaterial;

	Dictionary<Vector3, GameObject> buildingMap = new Dictionary<Vector3, GameObject>
	{
		{ new Vector3(-10f, 0, -40f), null },
		{ new Vector3(0, 0, -40f), null },
		{ new Vector3(-10f, 0, -30f), null },
		{ new Vector3(0, 0, -30f), null },
		{ new Vector3(-10f, 0, -20f), null },
		{ new Vector3(0, 0, -20f), null },
		{ new Vector3(-20f, 0, -10f), null },
		{ new Vector3(-10f, 0, -10f), null },
		{ new Vector3(0, 0, -10f), null },
		{ new Vector3(10f, 0, -10f), null },
		{ new Vector3(-20f, 0, 0), null },
		{ new Vector3(-10f, 0, 0), null },
		{ new Vector3(0f, 0, 0f), null },
		{ new Vector3(10f, 0, 0), null },
		{ new Vector3(-10f, 0, 30f), null },
		{ new Vector3(0, 0, 30f), null },
		{ new Vector3(-10f, 0, 20f), null },
		{ new Vector3(0, 0, 20f), null },
		{ new Vector3(-10f, 0, 10f), null },
		{ new Vector3(0, 0, 10f), null },
	};

	bool BuildModeOn = false;

	public void Off()
	{
		BuildModeOn = false;
	}

	public void On()
	{
		BuildModeOn = true;
	}

	void Start()
	{
		CreateBuildingLocations();
	}
	
	void Update()
	{
		if (!BuildModeOn)
		{
			return;
		}
		
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
			var position =  map.Key + mapOffset;
			Instantiate(BuildingSpaceIndicatorPrefab, position, Quaternion.identity, this.transform);
		}
	}

	void Delete()
	{
		var selected = WhatIsUnderTheMousePointer();
		if (selected.tag == BuildingTag)
		{
			ClearBuildingAt(selected.transform.position);
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
			Vector3 location;
			if (GetEmptyLocation(out location))
			{
				selectedBuilding = GameObject.CreatePrimitive(PrimitiveType.Capsule);
				selectedBuilding.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
				selectedBuilding.transform.position = location;
				selectedBuilding.tag = BuildingTag;
				var renderer = selectedBuilding.GetComponent<Renderer>();
				originalMaterial = renderer.sharedMaterial;
			}
		}
		else if (selected.tag == BuildingTag)
		{
			selectedBuilding = selected;
			ClearBuildingAt(selectedBuilding.transform.position);
			var renderer = selectedBuilding.GetComponent<Renderer>();
			originalMaterial = renderer.sharedMaterial;
			renderer.sharedMaterial = HighlightedMaterial;
		}
	}

	void MouseMove()
	{
		if (selectedBuilding != null)
		{
			Vector3 location;
			if (GetEmptyLocation(out location))
			{
				selectedBuilding.transform.position = location;
			}
		}
	}

	void MouseUp()
	{
		var renderer = selectedBuilding.GetComponent<Renderer>();
		renderer.sharedMaterial = originalMaterial;
		SaveBuilding(selectedBuilding.transform.position, selectedBuilding);
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

	void SaveBuilding(Vector3 worldPosition, GameObject building)
	{
		buildingMap[worldPosition - mapOffset] = building;
	}

	void ClearBuildingAt(Vector3 worldPosition)
	{
		buildingMap[worldPosition - mapOffset] = null;
	}

	bool GetEmptyLocation(out Vector3 location)
	{
		GameObject selected = WhatIsUnderTheMousePointer();
		if (selected != null && selected.tag == BuilderLocationTag)
		{
			var position = selected.transform.position - mapOffset;
			if (buildingMap.ContainsKey(position) && buildingMap[position] == null)
			{
				location = position + mapOffset;
				return true;
			}
		}
		location = Vector3.zero;
		return false;
	}

}
