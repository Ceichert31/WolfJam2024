using UnityEngine;

public abstract class ShipUnit : Unit
{
    public enum ShipUnitState
    {
        Detatched,
        Attached
    }

    [SerializeField]
    protected Health _myHealth;

    [SerializeField]
    private Rigidbody2D _rigidbody;

    [HideInInspector]
    protected ShipUnitState shipUnitState;

    public Health MyHealth { get { return _myHealth; } }
    public ShipUnitState MyShipUnitState { get { return shipUnitState; } }

    private void OnEnable()
    {
        _myHealth.OnDeath += DetatchUnit;
    }

    public override void Setup(UnitManager myUnitManager)
    {
        // cant have rigidbody
        if (_rigidbody != null) Destroy(_rigidbody);

        base.Setup(myUnitManager);

        shipUnitState = ShipUnitState.Attached;
    }

    /// <summary>
    /// occurs upon death
    /// </summary>
    public void DetatchUnit()
    {
        myUnitManager.RemoveUnit(this);
    }

    public override void HandleRemoval()
    {
        _rigidbody = gameObject.AddComponent<Rigidbody2D>();

        // set rigidbody to be moving freely
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.simulated = true;
        _rigidbody.gravityScale = 0;
        _rigidbody.freezeRotation = true;

        // set state to detached
        shipUnitState = ShipUnitState.Detatched;
    }
}
