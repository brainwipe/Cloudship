using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureBase : MonoBehaviour, IHaveAbilities, IStoreFlotsam
{
    public float TotalFlotsam
    {
        get
        {
            return totalFlotsam;
        }
        private set
        {
            totalFlotsam = value;
        }
    }

    public float MaxFlot;

    public float MaxFlotsam => MaxFlot;

    public Abilities Abilities;
    public float totalFlotsam;

    public Abilities Skills => Abilities;

    public bool IsBuilding => false;

    public bool IsFull => TotalFlotsam == MaxFlotsam;

    public float Add(float flotsam)
    {
        TotalFlotsam += flotsam;

        var remainder = 0f;
        if (TotalFlotsam > MaxFlotsam)
        {
            remainder = TotalFlotsam - MaxFlotsam;
            TotalFlotsam = MaxFlotsam;
        }
        return remainder;
    }

    public float Subtract(float flotsam)
    {
        TotalFlotsam -= flotsam;

        var remainder = 0f;
        if (TotalFlotsam < 0)
        {
            remainder = Mathf.Abs(TotalFlotsam);
            TotalFlotsam = 0;
        }
        return remainder;
    }
}