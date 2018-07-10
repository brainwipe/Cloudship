using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureBase : MonoBehaviour, IHaveAbilities, IStoreFlotsam
{
    public float TotalFlotsam { get; set; }

    public float MaxFlot;

    public float MaxFlotsam => MaxFlot;

    public Abilities Abilities;

    public Abilities Skills => Abilities;

}