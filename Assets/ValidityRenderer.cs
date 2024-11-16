using UnityEngine;

public class ValidityRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private float _oscSpeed = 1.0f;

    private void Update()
    {
        float alpha = 0.0f;

        // get alpha based on sine
        alpha = (Mathf.Sin(Time.time * _oscSpeed) * 0.5f) + 0.5f;

        _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, alpha);
    }
}
