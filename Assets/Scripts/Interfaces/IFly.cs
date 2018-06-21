using UnityEngine;

public interface IFly
{
    void ForceMovement(Rigidbody rigidBody, float torque, float speed);

    void Dead();
}