using UnityEngine;

public abstract class ShipUnit : Unit
{
    [SerializeField]
    protected Health _myHealth;

    public Health MyHealth { get { return _myHealth; } }

    private void OnEnable()
    {
        _myHealth.OnDeath += DetatchUnit;
    }

    /// <summary>
    /// occurs upon death
    /// </summary>
    public void DetatchUnit()
    {
        transform.parent = null;

        myUnitManager.RemoveUnit(this);
    }
}
