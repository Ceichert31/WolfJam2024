using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _myRenderer;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if(GameManager.Instance.GameState == GameManager.EGameState.Playing)
        {
            _myRenderer.enabled = false;
        }
        else
        {
            _myRenderer.enabled = true;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        transform.position = mousePos;
    }
}
