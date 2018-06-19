using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureBase : MonoBehaviour, IHaveAbilities
{
    public Abilities Abilities;

    public Abilities Skills => Abilities;
}