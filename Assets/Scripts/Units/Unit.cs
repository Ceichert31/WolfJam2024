using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
    public enum ShipUnitState
    {
        Detatched,
        Attached
    }

    [SerializeField]
    private Collider2D _myCollider;

    private Rigidbody2D _rigidbody;

    [HideInInspector] protected UnitManager myUnitManager;
    [HideInInspector] protected ShipUnitState shipUnitState;

    public ShipUnitState MyShipUnitState { get { return shipUnitState; } }

    public virtual void Setup(UnitManager myUnitManager)
    {
        if (_rigidbody != null)
        {
            Debug.Log("destroying rb");
            Destroy(_rigidbody);
        }

        this.myUnitManager = myUnitManager;

        shipUnitState = ShipUnitState.Attached;
        _myCollider.enabled = true;
    }

    public List<Unit> GetUnitNeighbors()
    {
        // there will be 8 neighboring tiles (null tiles are null)
        List<Unit> units = new List<Unit>();

        for(int i = 1; i >= -1; i--)
        {
            for(int j = -1; j <= 1; j++)
            {
                // get if possible and add to list
                if(myUnitManager.TryGetUnitAtPosition(transform.position + new Vector3(j, i), out Unit unit))
                {
                    units.Add(unit);
                }
                else
                {
                    units.Add(null);
                }
            }
        }

        return units;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.EGameState.Playing)
        {
            _myCollider.enabled = true;
        }
        else if(shipUnitState != ShipUnitState.Detatched)
        {
            _myCollider.enabled = false;
        }
    }

    public void DetatchUnit()
    {
        myUnitManager.RemoveUnit(this);

        _myCollider.enabled = false;
    }

    public virtual void UpdateUnit() { }

    public virtual void HandleRemoval()
    {
        // set state to detached
        shipUnitState = ShipUnitState.Detatched;

        _rigidbody = gameObject.AddComponent<Rigidbody2D>();

        // set rigidbody to be moving freely
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.simulated = true;
        _rigidbody.gravityScale = 0;
        _rigidbody.freezeRotation = true;
    }

    public void OnMouseEnter()
    {
        if (!DetatchedUnitHandler.instance.CanSelectUnits) return;
    }

    public void OnMouseDown()
    {
        DetatchedUnitHandler.instance.SetSelectedUnit(this);
    }

    public void OnMouseUp()
    {
        DetatchedUnitHandler.instance.SetSelectedUnit(null);
    }
}
