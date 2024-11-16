using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetatchedUnitHandler : MonoBehaviour
{
    [SerializeField]
    private UnitCursor _cursorPrefab;


    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateUpdated;
    }

    private void GameStateUpdated(GameManager.EGameState gameState)
    {
        if (gameState != GameManager.EGameState.Building)
        {
            Cleanup();

            return;
        }

        List<Unit> units = GetAllUnits();

        // instantiate cursors
        foreach(Unit unit in units)
        {
            Instantiate(_cursorPrefab.gameObject, unit.transform.position, Quaternion.identity);

            _cursorPrefab.SetUnit(unit);
        }
    }

    private void Cleanup()
    {
        Debug.Log("fuck 1");

        // destroy all cursors
        foreach (GameObject cursor in GameObject.FindGameObjectsWithTag("Cursor"))
        {

            Debug.Log("fuck 2");
            Destroy(cursor);
        }
    }

    private List<Unit> GetAllUnits()
    {
        List<Unit> units = new List<Unit>();

        for(int i = 0; i < transform.childCount; i++)
        {
            // add if has unit
            if (transform.GetChild(i).TryGetComponent(out Unit child))
            {
                units.Add(child);
            }
        }

        return units;
    }
}
