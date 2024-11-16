using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetatchedUnitHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _cursorPrefab;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateUpdated;
    }

    private void GameStateUpdated(GameManager.EGameState gameState)
    {
        if (gameState != GameManager.EGameState.Building) return;

        List<Unit> units = GetAllUnits();

        // instantiate cursors
        foreach(Unit unit in units)
        {
            Instantiate(_cursorPrefab, unit.transform.position, Quaternion.identity);
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
