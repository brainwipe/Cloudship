using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurface : MonoBehaviour
{
	Cloudship player;
	Building selectedBuilding;
	BuildMenu buildMenu;
	public MeshFilter Boundary;
	public Vector3 buildingToGrabPointOffset = Vector3.zero;

	void Awake()
	{
		if (buildMenu == null)
		{
			buildMenu = FindObjectOfType<BuildMenu>();
		}
		if (player == null)
		{
			player = FindObjectOfType<Cloudship>();
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
					selectedBuilding.Selected();
					buildingToGrabPointOffset = FindGrabPoint(selectedBuilding);
				}
				else
				{
					selectedBuilding = buildMenu.SelectedBuilding.Clone(transform);
					Vector3 localPosition;
					if (GetDesired(out localPosition))
					{
						selectedBuilding.transform.localPosition = localPosition;
					}
					else{
						selectedBuilding.transform.localPosition = Vector3.zero;
					}
					buildingToGrabPointOffset = Vector3.zero;
					SetBoundary(selectedBuilding);
				}
			}
		}

		if (Input.GetMouseButton(0))
		{
			if (selectedBuilding != null)
			{
				if (Input.GetKey(KeyCode.Q))
				{
					selectedBuilding.RotateAntiClockwise();
				}
				else if (Input.GetKey(KeyCode.E))
				{
					selectedBuilding.RotateClockwise();
				}
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
			buildingToGrabPointOffset = Vector3.zero;
			PlaceBuilding();
		}
	}

	void MouseMove()
	{
		if (selectedBuilding == null)
		{
			return;
		}	

		MoveSelectedBuilding();
	}

	void PlaceBuilding()
	{
		if (selectedBuilding != null && !selectedBuilding.CanPlace)
		{
			selectedBuilding.Remove();
		}
		selectedBuilding.UnSelected();
		selectedBuilding = null;
		player.UpdateAbilities();
	}

	void MoveSelectedBuilding()
	{
		Vector3 localPosition;
		if (GetDesired(out localPosition))
		{
			selectedBuilding.transform.localPosition = Vector3.Lerp(
				selectedBuilding.transform.localPosition, 
				localPosition, 
				Time.deltaTime * 40);
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

	bool GetDesired(out Vector3 localPosition)
	{
		localPosition = Vector3.zero;
		RaycastHit hit;

		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		 Debug.DrawRay(mouseRay.origin, mouseRay.direction * 1000, Color.blue, 1);
		
		var distance = ((selectedBuilding.transform.position + buildingToGrabPointOffset) - mouseRay.origin).magnitude;
		mouseRay.direction = (mouseRay.direction * distance) - buildingToGrabPointOffset;
		
		Debug.DrawRay(mouseRay.origin, mouseRay.direction * 1000, Color.green, 1);
		
		int layerMask = 1 << 8;

		if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, layerMask))
		{
			var toLocal = transform.InverseTransformPoint(hit.point);
			var zeroedY = new Vector3(toLocal.x, 0, toLocal.z);
			Debug.DrawLine(mouseRay.origin, zeroedY, Color.red, 1);
			localPosition = zeroedY;
			return true;
		}
		return false;
	}

	Vector3 FindGrabPoint(Building selectedBuilding)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 10;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			return hit.point - selectedBuilding.transform.position;
		}
		return Vector3.zero;
	}

	void SetBoundary(Building building)
	{
		float safetyMargin = 2f;

		Boundary.transform.localScale = Vector3.one;
		var boundaryExtents = Boundary.mesh.bounds.extents;
		var buildingBounds = building.PreCalculatedBounds;
		var buildingSizes = (buildingBounds.extents + new Vector3(safetyMargin, 0, safetyMargin));

		var xScale = (boundaryExtents.x - buildingSizes.x) / boundaryExtents.x;
		var zScale = (boundaryExtents.z - buildingSizes.z) / boundaryExtents.z;
		Boundary.transform.localScale = new Vector3(xScale, 1, zScale);
	}

	public List<SaveGame.BuildingSave> Save()
	{
		var buildings = GetComponentsInParent<Building>();
		var savedBuildings = new List<SaveGame.BuildingSave>();

		foreach(var building in buildings)
		{
			savedBuildings.Add(new SaveGame.BuildingSave(building));
		}
		return savedBuildings;
	}

	public void Load(List<SaveGame.BuildingSave> buildings)
	{
		foreach(var savedBuilding in buildings)
		{
			var prefab = BuildingLoader.Load(savedBuilding.Name);
			var gameObject = Instantiate(prefab, transform.position, Quaternion.identity, this.transform);
			gameObject.transform.localPosition = savedBuilding.LocalPosition;
			gameObject.transform.localRotation = savedBuilding.LocalRotation;
			var building = gameObject.GetComponent<Building>();
			building.Health = savedBuilding.Health;

		}
	}
}
