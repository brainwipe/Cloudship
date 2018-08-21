using UnityEngine;

public interface IFly
{
    float CommandThrust { get; }
    float CommandTurn { get; }
    void Dead();
}