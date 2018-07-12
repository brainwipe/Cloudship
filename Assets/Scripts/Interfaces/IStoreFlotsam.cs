using System;
public interface IStoreFlotsam
{
    float TotalFlotsam { get; }

    float MaxFlotsam { get; }

    bool IsBuilding { get; }

    bool IsFull { get; }

    void Store(float flotsam);
}