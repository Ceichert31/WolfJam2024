using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
    [HideInInspector] protected UnitManager myUnitManager;

    public virtual void Setup(UnitManager myUnitManager)
    {
        this.myUnitManager = myUnitManager;
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

    public virtual void UpdateUnit() { }

    public virtual void HandleRemoval() { }
}
