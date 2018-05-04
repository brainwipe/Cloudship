using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour {

	public GameObject BuildingSpaceIndicatorPrefab;

	Building selectedBuilding;
	BuildMenu buildMenu;

	void Awake()
	{
		if (buildMenu == null)
		{
			buildMenu = FindObjectOfType<BuildMenu>();
		}
	}

	void OnDisable()
	{
	}

	void OnEnable()
	{
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
				// TODO
			}
		}
	
		if(Input.GetMouseButtonUp(0))
		{
			MouseUp();
		}
	}

	void MouseDown()
	{
		var selected = WhatIsUnderTheMousePointer();
		if (selected == null)
		{
			return;
		}
		else if (selected.tag == "Player")
		{
			if (selectedBuilding == null)
			{
				selectedBuilding = buildMenu.SelectedBuilding.Clone(transform);
				Debug.Log(GetPlace());
				var place = GetPlace();
				selectedBuilding.transform.position = new Vector3(place.x, selected.transform.position.y, place.z);
				selectedBuilding.ToggleHighlight();
			}
		}
	}

	void MouseMove()
	{
		var selected = WhatIsUnderTheMousePointer();

		if (selected == null)
		{
			return;
		}

		
	}

	void MouseUp()
	{
		if (selectedBuilding != null)
		{
			var building = selectedBuilding.GetComponent<Building>();
			selectedBuilding.ToggleHighlight();
			SaveBuilding(building);
		}
	}

	Vector3 GetPlace()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			return hit.point;
		}
		return Vector3.zero;
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
		selectedBuilding = null;
	}
}
