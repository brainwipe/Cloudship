using UnityEngine;

public interface IFly
{
    float CommandThrust { get; }
    float CommandTurn { get; }
    Vector3 DesiredThrust();
    Vector3 DesiredTorque();
    void Dead();
}