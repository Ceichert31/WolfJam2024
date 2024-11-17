using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Health))]
public class UnitManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Grid _myGrid;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private Transform _unitHolders;

    private Transform _testDetachedUnitHolder;

    [SerializeField]
    private int _maxHealthIncreasePerUnit = 50;

    [SerializeField]
    private bool _isPlayerShip = false;

    //public bool IsTestingMode = false;
    //public bool IsTestingUnitDeath = false;

    private Health _myHealth;

    [Header("Audio References")]
    private AudioSource _audioSource;
    [SerializeField] private AudioPitcherSO deathPitcher;

    // Getters
    public Transform DetachedGridHolder { get { return _testDetachedUnitHolder; } }
    private List<Unit> _units = new List<Unit>();
    public List<Unit> Units { get { return _units; } }
    public Grid MyGrid { get { return _myGrid; } }
    public Health MyHealth { get { return _myHealth; } }
    public bool IsPlayerShip { get { return _isPlayerShip; } }


    private void Start()
    {
        // get all units from the unit holder
        for(int i = 0; i < _unitHolders.childCount; i++)
        {
            if (_unitHolders.GetChild(i).TryGetComponent(out Unit unit))
            {
                _units.Add(unit);

                unit.Setup(this);
            }
        }

        _testDetachedUnitHolder = FindFirstObjectByType<DetatchedUnitHandler>().gameObject.transform;

        //if(IsTestingMode) RemoveUnit(_units[0]);

        _myHealth = GetComponent<Health>();

        _myHealth.OnDeath += Death;
        _myHealth.OnHealthUpdate += HealthUpdate;
    }

    public void Death()
    {
        Debug.Log("Unit Death");

        if (_isPlayerShip)
        {
            RemoveAllUnits();

            GameManager.Instance.UpdateGameState(GameManager.EGameState.Lose);
        }
        else
        {
            RemoveAllUnits();
            GameManager.Instance.AddToKills();

            //Play death audio
            deathPitcher.Play(_audioSource);

            Destroy(gameObject);
        }
    }

    private void RemoveAllUnits()
    {
        // detatch all
        while (_units.Count > 0)
        {
            Unit u = _units[0];
            RemoveUnit(_units[0]);
            u.ExplodeFromPoint(GetShipCenterPoint());
        }
    }

    public void HealthUpdate(int oldHealth, int newHealth)
    {
        if(oldHealth > newHealth)
        {
            // damage was taken
        }

        if(newHealth > oldHealth)
        {
            // health gained
        }
    }

    public void AddUnit(Unit unit)
    {
        _myHealth.IncreaseMaxHealth(_maxHealthIncreasePerUnit);
        _units.Add(unit);
        unit.transform.parent = _unitHolders.transform;
        unit.Setup(this);
        //Change enemy layer and unit layer
        //Function that returns whether something is part of player
        //unit.SetTargetEnemyLayer(1 << LayerMask.NameToLayer("Enemy"));
    }

    public bool CanAddUnit(Vector2 worldPos, Unit unit)
    {
        // unit here, cant add
        if (IsUnitInTheWay(worldPos))
        {
            Debug.Log("theres a unit in the way");
            return false;
        }
        if (!IsConnected(worldPos)) return false;

        return true;
    }

    public bool IsUnitInTheWay(Vector2 worldPos)
    {
        if (TryGetUnitAtPosition(worldPos, out Unit u))
        {
            Debug.Log("theres a unit in the way");
            return true;
        }

        return false;
    }

    public bool IsConnected(Vector3 worldPos)
    {
        // there will be 8 neighboring tiles (null tiles are null)
        List<Unit> units = new List<Unit>();

        for (int i = 1; i >= -1; i--)
        {
            for (int j = -1; j <= 1; j++)
            {
                // get if possible and add to list
                if (TryGetUnitAtPosition(worldPos + new Vector3(j, i), out Unit unit))
                {
                    units.Add(unit);
                }
                else
                {
                    units.Add(null);
                }
            }
        }

        // 4 is the center. 1 is up, 3 is left, 5 is ri
        if (units[1] != null || units[3] != null || units[5] != null || units[7] != null)
        {
            return true;
        }

        return false;
    }

    public void RemoveUnit(Unit unit)
    {
        if (!_units.Contains(unit)) return;

        _tilemap.SetTile(_myGrid.WorldToCell(unit.transform.position), null);
        _units.Remove(unit);

        unit.transform.parent = _testDetachedUnitHolder;
        unit.HandleRemoval();

        Debug.Log("Unit at " + transform.position + " has been removed!");
    }

    public bool TryGetUnitAtPosition(Vector2 worldPos, out Unit unit)
    {
        unit = null;

        foreach(Unit u in _units)
        {
            // tolerance of 0.1 just in case
            if(Vector2.Distance(worldPos, u.transform.position) < 0.1f)
            {
                unit = u;
                return true;
            }
        }

        return false;
    }

    public Vector2 GetExtents()
    {
        // updated with max values
        Vector2 extents = new Vector2(0, 0);

        foreach(Unit unit in _units)
        {
            // extend based on maximum x
            if(Mathf.Abs(unit.transform.position.x) > extents.x)
            {
                extents.x = Mathf.Abs(unit.transform.position.x);
            }

            // extend based on maximum y
            if(Mathf.Abs(unit.transform.position.y) > extents.y)
            {
                extents.y = Mathf.Abs(unit.transform.position.y);
            }
        }

        return extents;
    }

    public Vector2 GetShipCenterPoint()
    {
        Vector2 all = Vector2.zero;
        int count = 0;

        foreach(Unit unit in _units)
        {
            all.x += unit.transform.position.x;
            all.y += unit.transform.position.y;

            count++;
        }

        if(count != 0) {
            all = new Vector2(all.x / count, all.y / count);
        }

        return all;
    }
}
