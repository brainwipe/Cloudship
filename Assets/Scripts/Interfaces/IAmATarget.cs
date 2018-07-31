using UnityEngine;

public interface IAmATarget
{
    bool IsDead { get; }

    Vector3 Position { get; }

    Vector3 Velocity { get; }
}