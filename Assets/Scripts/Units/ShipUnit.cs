using UnityEngine;

public abstract class ShipUnit : Unit
{
    [SerializeField]
    protected Health _myHealth;

    public Health MyHealth { get { return _myHealth; } }
}
