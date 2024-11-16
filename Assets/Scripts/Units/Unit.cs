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

    [SerializeField]
    private float _detatchementForce = 4.0f;

    private Rigidbody2D _rigidbody;

    [HideInInspector] protected UnitManager myUnitManager;
    protected ShipUnitState shipUnitState;

    public ShipUnitState MyShipUnitState { get { return shipUnitState; } }
    public UnitManager MyUnitManager { get { return myUnitManager; } }

    public virtual void Setup(UnitManager myUnitManager)
    {
        if (_rigidbody != null)
        {
            Debug.Log("destroying rb");
            Destroy(_rigidbody);
        }


        this.myUnitManager = myUnitManager;

        if (MyUnitManager.IsPlayerShip) gameObject.layer = LayerMask.NameToLayer("Player");
        else gameObject.layer = LayerMask.NameToLayer("Enemy");

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
        _rigidbody.linearDamping = 0.5f;

        gameObject.layer = LayerMask.NameToLayer("Default");

        //Vector2 dir = Random.insideUnitCircle;
        //dir.Normalize();

        //_rigidbody.AddForce(dir * _detatchementForce, ForceMode2D.Impulse);
    }

    public void ExplodeFromPoint(Vector2 point)
    {
        _rigidbody.AddForceAtPosition((new Vector2(transform.position.x, transform.position.y) - point).normalized * _detatchementForce, point, ForceMode2D.Impulse);
    }

    public void OnMouseEnter()
    {
        if (!DetatchedUnitHandler.instance.CanSelectUnits) return;

        // hover
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
