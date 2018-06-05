using System;

[Serializable]
public class Abilities
{
    public float Torque;
    public float Speed;
    public bool GiveOrders;
}

public interface IHaveAbilities
{
    Abilities Skills {get;}
}