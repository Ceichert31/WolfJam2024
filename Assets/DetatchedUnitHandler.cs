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
    private Unit lastUnitHeld;

    private Dictionary<Unit, UnitCursor> cursorToUnitPair = new Dictionary<Unit, UnitCursor>();
    public bool CanSelectUnits { get { return canSelectUnits; } }

    [Header("Audio References")]
    [SerializeField] private AudioPitcherSO _attachPitcher;
    private AudioSource _audioSource => Camera.main.GetComponent<AudioSource>();

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
            unit.gameObject.layer = LayerMask.NameToLayer("BuilderModeUnit");

            cursor.SetUnit(unit);
            cursorToUnitPair.Add(unit, cursor);
            cursor.ShowCursor();
            cursor.HideValidityCheck();
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

        List<Unit> units = GetAllUnits();

        UnitManager unitManager = GameManager.Instance.Player.GetComponent<UnitManager>();

        foreach (Unit u in units)
        {
            if (unitManager.CanAddUnit(u.transform.position, u))
            {
                GameManager.Instance.AddToConnections();

                //Play sound effect
                _attachPitcher.Play(_audioSource);
                unitManager.AddUnit(u);
            }
            else
            {
                Destroy(u.gameObject);
            }
        }

        selectedUnit = null;
    }

    private void Update()
    {
        UpdateSelectedUnitPosition();
    }

    private void UpdateSelectedUnitPosition()
    {
        if (GameManager.Instance.GameState != GameManager.EGameState.Building) return;
        if (selectedUnit == null)
        {
            if(lastUnitHeld != null && cursorToUnitPair.ContainsKey(lastUnitHeld)) cursorToUnitPair[lastUnitHeld].HideValidityCheck();
            return;
        }

        Vector3 point = GetGridPosition();
        selectedUnit.transform.position = point;

        // show validity on screen
        UnitManager unitManager = GameManager.Instance.Player.GetComponent<UnitManager>();

        if (!unitManager.IsUnitInTheWay(point) && !unitManager.IsConnected(point))
        {
            //cursorToUnitPair[selectedUnit].HideValidityCheck();
            cursorToUnitPair[selectedUnit].Holding();
        }
        else {
            cursorToUnitPair[selectedUnit].ShowValidityCheck(unitManager.CanAddUnit(point, selectedUnit));
        }
    }

    private Vector3 GetGridPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        //Vector3 decimalRemainder = new Vector2(GameManager.Instance.Player.transform.position.x % 1, GameManager.Instance.Player.transform.position.y % 1);
        Vector3Int closestPoint = GameManager.Instance.Player.GetComponent<Grid>().WorldToCell(mousePos);
        Vector3 point = closestPoint + GameManager.Instance.Player.transform.position + new Vector3(0.5f, 0.5f);

        return point;
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
        if (GameManager.Instance.GameState != GameManager.EGameState.Building) return;

        selectedUnit = unit;

        if (unit == null) return;
        lastUnitHeld = selectedUnit;

        List<Unit> units = GetAllUnits();

        UnitCursor currentCursor = null;

        // destroy all others
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i] != unit)
            {
                // destroy and remove pairing
                Destroy(cursorToUnitPair[units[i]].gameObject);

                Destroy(units[i].gameObject);
            }
            else
            {
                currentCursor = cursorToUnitPair[units[i]];
            }
        }

        cursorToUnitPair.Clear();
        cursorToUnitPair.Add(unit, currentCursor);
    }
}
