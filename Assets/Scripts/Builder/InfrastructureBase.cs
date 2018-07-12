using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureBase : MonoBehaviour, IHaveAbilities, IStoreFlotsam
{
    public float TotalFlotsam { get; private set; }

    public float MaxFlot;

    public float MaxFlotsam => MaxFlot;

    public Abilities Abilities;

    public Abilities Skills => Abilities;

    public bool IsBuilding => false;

    public bool IsFull => TotalFlotsam == MaxFlotsam;

    public void Store(float flotsam)
    {
        TotalFlotsam += flotsam;
        if (TotalFlotsam > MaxFlotsam)
        {
            TotalFlotsam = MaxFlotsam;
        }
    }
}