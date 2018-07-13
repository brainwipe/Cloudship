using System.Linq;
using UnityEngine;

public class StoreManager
{
    private IStoreFlotsam[] allStores;

    public StoreManager(Transform cloudshipTransform)
    {
        allStores = cloudshipTransform.GetComponentsInChildren<IStoreFlotsam>();
    }

    IStoreFlotsam[] Buildings => allStores.Where(x => x.IsBuilding).ToArray();
    IStoreFlotsam Infrastructure => allStores.Single(x => !x.IsBuilding);

    internal void AddFlotsam(float value)
    {
        value = Infrastructure.Store(value);

        foreach(var building in Buildings.OrderByDescending(x => x.TotalFlotsam))
        {
            if (value < 1)
            {
                return;
            }
            value = building.Store(value);
        }
    }

    internal void RemoveFlotsam(float value)
    {
        foreach(var building in Buildings.OrderBy(x => x.TotalFlotsam))
        {
            if (value < 1)
            {
                return;
            }
            value = building.Store(-value);
        }

        if (value < 1)
        {
            return;
        }

        Infrastructure.Store(-value);
        Debug.Log($"Total Flotsam: {TotalFlotsam}");
    }

    internal float TotalFlotsam => allStores.Sum(x => x.TotalFlotsam);

    internal float MaxFlotsam => allStores.Sum(x => x.MaxFlotsam);

    internal bool IsFull => allStores.All(x => x.IsFull);

    public float InfrastructreFlotsam 
    {
        get
        {
            return Infrastructure.TotalFlotsam;
        }
        set
        {
            Infrastructure.Store(value);
        }
    }
    
}