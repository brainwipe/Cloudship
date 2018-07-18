using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSmall : MonoBehaviour, IStoreFlotsam
{
    public float MaxFlot;

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

    public float MaxFlotsam => MaxFlot;

    public bool IsBuilding => true;

    public bool IsFull => TotalFlotsam == MaxFlotsam;

    public Transform Indicator;
    float indicatorLevelSpeed = 4f;
    public float totalFlotsam;

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

    void Update()
    {
        var indicatorLevel = Maths.Rescale(0, 4f, 0, MaxFlotsam, TotalFlotsam);
        var newPosition = new Vector3(0, indicatorLevel, 0);
        Indicator.localPosition = Vector3.Lerp(Indicator.localPosition, newPosition, Time.deltaTime * indicatorLevelSpeed);
    }
}
