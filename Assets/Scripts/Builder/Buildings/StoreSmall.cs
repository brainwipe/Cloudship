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

    public float Store(float flotsam)
    {
        if (TotalFlotsam < 0)
        {
            return flotsam;
        }

        TotalFlotsam += flotsam;
        float remainder = 0;
        if (TotalFlotsam > MaxFlotsam)
        {
            remainder = TotalFlotsam - MaxFlotsam;
            TotalFlotsam = MaxFlotsam;
        }
        else if (TotalFlotsam < 0)
        {
            remainder = TotalFlotsam;
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
