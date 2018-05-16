using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour 
{
	Building selectedBuilding;
	BuildMenu buildMenu;
	public MeshFilter Boundary;
	public Vector3 offset = Vector3.zero;

	void Awake()
	{
		if (buildMenu == null)
		{
			buildMenu = FindObjectOfType<BuildMenu>();
		}
	}

	void Update()
	{
		MouseMove();
		
		if (Input.GetMouseButtonDown(0))
		{
			if (selectedBuilding == null)
			{
				Building building;
				if (IsThereABuildingUnderMousePointer(out building))
				{
					selectedBuilding = building;
					offset = FindOffset(selectedBuilding);
				}
				else
				{
					selectedBuilding = buildMenu.SelectedBuilding.Clone(transform);
					offset = Vector3.zero;
				}
				SetBoundary(selectedBuilding);
			}
		}

		if (Input.GetMouseButton(0))
		{
			if (Input.GetKeyUp(KeyCode.Delete))
			{
				// TODO ROLA Delete
			}
		}
	
		if(Input.GetMouseButtonUp(0))
		{
			offset = Vector3.zero;
			PlaceBuilding();
		}
	}

	void MouseMove()
	{
		if (selectedBuilding == null)
		{
			return;
		}	

		if(buildMenu.SelectedBuilding.Name != selectedBuilding.Name) 
		{
			selectedBuilding.Remove();
			selectedBuilding = null;
		}

		MoveSelectedBuilding();
	}

	void PlaceBuilding()
	{
		if (selectedBuilding != null && !selectedBuilding.CanPlace)
		{
			selectedBuilding.Remove();
		}
		selectedBuilding = null;
	}

	void MoveSelectedBuilding()
	{
		Vector3 place;
		if (GetDesired(out place))
		{
			var zeroedY = new Vector3(place.x, 0, place.z);
			selectedBuilding.transform.position = zeroedY;
			selectedBuilding.IsOverCloudship = true;
		}
		else 
		{
			selectedBuilding.IsOverCloudship = false;
		}
		selectedBuilding.UpdateVisibility();
	}

	bool IsThereABuildingUnderMousePointer(out Building building)
	{
		RaycastHit hit;
		building = null;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 10;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			if (hit.transform.tag == "Building")
			{
				building = hit.transform.GetComponentInParent<Building>();
				return true;
			}
			else
			{
				Debug.Log(hit.transform.tag + " " + hit.transform.gameObject.name);
			}
			return false;
		}
		return false;
	}

	bool GetDesired(out Vector3 position)
	{
		RaycastHit hit;
		position = Vector3.zero;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Debug.DrawRay(ray.origin, ray.direction * 100000, Color.green, 1);

		int layerMask = 1 << 8;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			position = hit.point;
			return true;
		}
		return false;
	}

	Vector3 UnfilteredMousePosition()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			return hit.point;
		}
		return Vector3.zero;
	}

	Vector3 FindOffset(Building building)
	{
		Vector3 mouseClickedLocation = UnfilteredMousePosition();
		Vector3 newOffset = Vector3.zero;
		
		newOffset = mouseClickedLocation - building.transform.position;
		
		return newOffset;
	}

	void SetBoundary(Building building)
	{
		float safetyMargin = 1f;

		Boundary.transform.localScale = Vector3.one;
		var boundaryExtents = Boundary.mesh.bounds.extents;
		var buildingMeshFilter = building.GetComponent<MeshFilter>();
		var buildingSizes = (buildingMeshFilter.mesh.bounds.extents + new Vector3(safetyMargin, 0, safetyMargin));

		var xScale = (boundaryExtents.x - buildingSizes.x) / boundaryExtents.x;
		var zScale = (boundaryExtents.z - buildingSizes.z) / boundaryExtents.z;
		Boundary.transform.localScale = new Vector3(xScale, 1, zScale);
	}
}
