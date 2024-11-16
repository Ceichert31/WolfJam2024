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
    }

    public void AddUnit()
    {
        foreach(Unit unit in _units)
        {
            unit.UpdateUnit();
        }
    }

    public bool CanAddUnit(Vector2 worldPos)
    {


        return true;
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
