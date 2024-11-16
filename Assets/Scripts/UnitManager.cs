using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private Grid _myGrid;

    [SerializeField]
    private Transform _unitHolders;

    private List<Unit> _units = new List<Unit>();
    public List<Unit> Units { get { return _units; } }

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

        if (units[0] != null)
        {
            Debug.Log("Upper Left:" + units[0] + " pos: " + units[0].transform.position);
        }

        if (units[1] != null)
        {
            Debug.Log("Upper Middle:" + units[1] + " pos: " + units[1].transform.position);
        }


        if (units[2] != null)
        {
            Debug.Log("Upper Right:" + units[2] + " pos: " + units[2].transform.position);
        }

        if (units[3] != null)
        {
            Debug.Log("Center Left:" + units[3] + " pos: " + units[3].transform.position);
        }

        if (units[4] != null)
        {
            Debug.Log("Center" + units[4] + " pos: " + units[4].transform.position);
        }

        if (units[5] != null)
        {
            Debug.Log("Center Right:" + units[5] + " pos: " + units[5].transform.position);
        }
        if (units[6] != null)
        {
            Debug.Log("Lower Left:" + units[6] + " pos: " + units[6].transform.position);
        }
        if (units[7] != null)
        {
            Debug.Log("Lower Middle:" + units[7] + " pos: " + units[7].transform.position);
        }
        if (units[8] != null)
        {
            Debug.Log("Lower Right:" + units[8] + " pos: " + units[8].transform.position);
        }













    }

    public void AddUnit()
    {

    }

    public bool CanAddUnit()
    {
        return true;
    }

    public void RemoveUnit()
    {

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
