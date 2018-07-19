using System;

[Serializable]
public class Abilities
{
    public float Torque;
    public float Thrust;
    public bool GiveOrders;
    public float Mass;
    public float Lift;
    public float Health;
    public float TopSpeed;
}

public interface IHaveAbilities
{
    Abilities Skills {get;}
}