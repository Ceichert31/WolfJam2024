using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetatchedUnitHandler : MonoBehaviour
{
    public static DetatchedUnitHandler instance;

    [SerializeField]
    private UnitCursor _cursorPrefab;

    private bool canSelectUnits;

    public Unit selectedUnit;

    private Dictionary<Unit, UnitCursor> cursorToUnitPair = new Dictionary<Unit, UnitCursor>();
    public bool CanSelectUnits { get { return canSelectUnits; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateUpdated;
    }

    private void GameStateUpdated(GameManager.EGameState gameState)
    {
        if (gameState != GameManager.EGameState.Building)
        {
            Cleanup();
            canSelectUnits = false;

            return;
        }

        cursorToUnitPair.Clear();
        List<Unit> units = GetAllUnits();

        // instantiate cursors
        foreach(Unit unit in units)
        {
            UnitCursor cursor = Instantiate(_cursorPrefab, unit.transform.position, Quaternion.identity);

            cursor.SetUnit(unit);
            cursorToUnitPair.Add(unit, cursor);
            cursor.ShowCursor();
        }

        // STOP
        foreach(Rigidbody2D rb in GameObject.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None))
        {
            rb.linearVelocity = Vector2.zero;
        }

        canSelectUnits = true;
    }

    private void Cleanup()
    {
        // destroy all cursors
        foreach (GameObject cursor in GameObject.FindGameObjectsWithTag("Cursor"))
        {
            Destroy(cursor);
        }
    }

    private void Update()
    {
        UpdateSelectedUnitPosition();
    }

    private void UpdateSelectedUnitPosition()
    {
        if (selectedUnit == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        //Vector3 decimalRemainder = new Vector2(GameManager.Instance.Player.transform.position.x % 1, GameManager.Instance.Player.transform.position.y % 1);
        Vector3Int closestPoint = GameManager.Instance.Player.GetComponent<Grid>().WorldToCell(mousePos);
        Vector3 point = closestPoint + GameManager.Instance.Player.transform.position + new Vector3(0.5f, 0.5f);

        selectedUnit.transform.position = point;
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

    public void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
    }
}
