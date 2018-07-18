using System.Linq;
using UnityEngine;

public class StoreManager
{
    IStoreFlotsam[] AllStores => GameManager.Instance.PlayerCloudship.transform.GetComponentsInChildren<IStoreFlotsam>();

    IStoreFlotsam[] Buildings => AllStores.Where(x => x.IsBuilding).ToArray();
    IStoreFlotsam Infrastructure => AllStores.Single(x => !x.IsBuilding);

    internal void AddFlotsam(float value)
    {
        value = Infrastructure.Add(value);

        foreach(var building in Buildings.OrderByDescending(x => x.TotalFlotsam))
        {
            if (value < 1)
            {
                return;
            }
            value = building.Add(value);
        }
    }

    internal void RemoveFlotsam(float value)
    {
        if (!GameManager.Instance.Mode.BuildingsCostFlotsam)
        {
            return;
        }

        foreach(var building in Buildings.OrderBy(x => x.TotalFlotsam))
        {
            if (value < 1)
            {
                return;
            }
            value = building.Subtract(value);
        }

        if (value < 1)
        {
            return;
        }

        Infrastructure.Subtract(value);
    }

    internal float TotalFlotsam => AllStores.Sum(x => x.TotalFlotsam);

    internal float MaxFlotsam => AllStores.Sum(x => x.MaxFlotsam);

    internal bool IsFull => AllStores.All(x => x.IsFull);

    public float InfrastructreFlotsam 
    {
        get
        {
            return Infrastructure.TotalFlotsam;
        }
        set
        {
            Infrastructure.Add(value);
        }
    }
    
}