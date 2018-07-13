using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSmall : MonoBehaviour, IStoreFlotsam 
{
	public float MaxFlot;

    public float TotalFlotsam { get; private set; }
    
    public float MaxFlotsam => MaxFlot;

    public bool IsBuilding => true;

    public bool IsFull => TotalFlotsam == MaxFlotsam;

    public float Store(float flotsam)
    {
        TotalFlotsam += flotsam;
        float remainder = 0;
        if (TotalFlotsam > MaxFlotsam)
        {
            remainder = TotalFlotsam - MaxFlotsam;
            TotalFlotsam = MaxFlotsam;
        }
        return remainder;
    }
	
	void Update () {
		
	}
}
