using UnityEngine;

public class UnitCursor : MonoBehaviour
{
    private Unit unit;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private SpriteRenderer _validityCheckRenderer;

    [SerializeField]
    private Color _validColor;

    [SerializeField]
    private Color _invalidColor;

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

    public void ShowValidityCheck(bool isValid)
    {
        _validityCheckRenderer.enabled = true;

        if (isValid)
        {
            _validityCheckRenderer.color = new Color(_validColor.r, _validColor.g, _validColor.b, _validityCheckRenderer.color.a);
        }
        else
        {
            _validityCheckRenderer.color = new Color(_invalidColor.r, _invalidColor.g, _invalidColor.b, _validityCheckRenderer.color.a);
        }
    }

    public void HideValidityCheck()
    {
        _validityCheckRenderer.enabled = false;
    }
}
