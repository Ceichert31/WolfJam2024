using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private Grid _myGrid;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private Transform _unitHolders;

    [SerializeField]
    private Transform _testDetachedUnitHolder;

    public bool IsTestingMode = false;

    // Getters
    public Transform DetachedGridHolder { get { return _testDetachedUnitHolder; } }
    private List<Unit> _units = new List<Unit>();
    public List<Unit> Units { get { return _units; } }
    public Grid MyGrid { get { return _myGrid; } }

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

        // test
        List<Unit> units = _units[0].GetUnitNeighbors();

        if(IsTestingMode) RemoveUnit(_units[0]);
    }

    public void AddUnit(Unit unit)
    {
        unit.Setup(this);

        foreach(Unit u in _units)
        {
            u.UpdateUnit();
        }
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
}
