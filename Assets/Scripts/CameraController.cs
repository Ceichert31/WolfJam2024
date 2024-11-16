using UnityEngine;

public class CameraController : MonoBehaviour
{
    private UnitManager unitManager;

    private Camera cam => Camera.main;

    Vector2 unitSize;

    private void Start()
    {
        unitManager = GetComponentInParent<UnitManager>();

        unitSize = unitManager.GetExtents();

        cam.orthographicSize = unitSize.x / 2;
    }

    private void Update()
    {

    }
}
