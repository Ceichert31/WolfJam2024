using UnityEngine;

public class UnitCursor : MonoBehaviour
{
    private Unit unit;

    [SerializeField]
    private SpriteRenderer _renderer;

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }

    private void Update()
    {
        transform.position = unit.transform.position;
    }

    public void HideCursor()
    {
        _renderer.enabled = false;
    }

    public void ShowCursor()
    {
        _renderer.enabled = true;
    }
}
