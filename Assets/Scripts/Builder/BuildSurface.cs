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

	void FixedUpdate()
	{
		if (GameManager.Instance.GameIsPaused)
		{
			return;
		}

		MoveSelectedBuilding();
		
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
				else if (buildMenu.SelectedBuilding.CanAfford(player.Stores.TotalFlotsam))
				{
					selectedBuilding = buildMenu.SelectedBuilding.Clone(transform);
					player.Stores.RemoveFlotsam(selectedBuilding.FlotsamCost);
					Vector3 position = Vector3.zero;
					if (GetDesired(out position))
					{	
						selectedBuilding.Position = position;
					}
					selectedBuilding.Rotation = transform.rotation;
					buildingToGrabPointOffset = Vector3.zero;
					SetBoundary(selectedBuilding);
				}
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
			PlaceBuilding();
		}
	}

	void OnDisable()
	{
		PlaceBuilding();
	}


	void MoveSelectedBuilding()
	{
		if (selectedBuilding == null)
		{
			return;
		}	

		Vector3 position;
		if (GetDesired(out position))
		{
			selectedBuilding.Position = position;

			var buildingTurning = Quaternion.Euler(0, selectedBuilding.transform.localEulerAngles.y, 0);
			if (Input.GetKey(KeyCode.Q))
			{
				buildingTurning = Quaternion.Euler(0, selectedBuilding.transform.localEulerAngles.y - 1, 0);
			}
			else if (Input.GetKey(KeyCode.E))
			{
				buildingTurning = Quaternion.Euler(0, selectedBuilding.transform.localEulerAngles.y + 1, 0);
			}

			selectedBuilding.Rotation = transform.rotation * buildingTurning;
			selectedBuilding.IsOverCloudship = true;
		}
		else 
		{
			selectedBuilding.IsOverCloudship = false;
		}
		selectedBuilding.UpdateVisibility();
	}

	void PlaceBuilding()
	{
		buildingToGrabPointOffset = Vector3.zero;
		if (selectedBuilding == null)
		{
			return;
		}

		if (!selectedBuilding.CanPlace)
		{
			var cost = selectedBuilding.FlotsamCost;
			selectedBuilding.Remove();
			player.Stores.AddFlotsam(cost);
		}
		selectedBuilding.UnSelected();
		selectedBuilding = null;
		player.UpdateAbilities();
	}

	bool IsThereABuildingUnderMousePointer(out Building building)
	{
		RaycastHit hit;
		building = null;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 10;
		
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.black, 0.5f);
			if (hit.transform.tag == "Building")
			{
				building = hit.transform.GetComponentInParent<Building>();
				return true;
			}
			return false;
		}
		else 
		{
			Debug.DrawRay(ray.origin, ray.direction * 1000, Color.white, 0.5f);
		}
		return false;
	}

	bool GetDesired(out Vector3 position)
	{
		position = Vector3.zero;
		RaycastHit hit;

		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Debug.DrawRay(mouseRay.origin, mouseRay.direction * 1000, Color.blue, 1);
		
		var distance = ((selectedBuilding.transform.position + buildingToGrabPointOffset) - mouseRay.origin).magnitude;
		mouseRay.direction = (mouseRay.direction * distance) - buildingToGrabPointOffset;
		
		//Debug.DrawRay(mouseRay.origin, mouseRay.direction * 1000, Color.green, 1);
		
		int layerMask = 1 << 8;
		if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, layerMask))
		{
			
			position = hit.point;
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
		var buildings = GetComponentsInChildren<Building>();
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
			var prefab = BuildingLoader.Find(savedBuilding.Id);
			var gameObject = Instantiate(prefab, transform.position, Quaternion.identity, this.transform);
			gameObject.transform.localPosition = savedBuilding.LocalPosition.ToVector();
			gameObject.transform.localRotation = savedBuilding.LocalRotation.ToQuaternion();
			var building = gameObject.GetComponent<Building>();
			building.Health = savedBuilding.Health;
			
		}
	}
}
