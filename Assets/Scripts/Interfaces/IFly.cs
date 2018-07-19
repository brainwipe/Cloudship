using UnityEngine;

public interface IFly
{
    Vector3 DesiredThrust();

    Vector3 DesiredTorque();

    void Dead();
}