using UnityEngine;
public interface IAmAShip
{
    Vector3 Position { get; }

    bool CanShoot { get; }

    string MyEnemyTagIs { get; }

    bool ShootFullAuto { get; }

    bool FireAtWill { get; }

    bool IAmAPlayer { get; }
    Vector3 Velocity { get; }

    void UpdateAbilities();
}