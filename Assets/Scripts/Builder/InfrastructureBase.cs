using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureBase : MonoBehaviour, IHaveAbilities, IStoreFlotsam
{
    public Abilities Abilities;

    public Abilities Skills => Abilities;

    public float Flotsam { get; set; }
}