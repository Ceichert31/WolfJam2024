using UnityEngine;

public class SimpleSpriteAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private float _timeBetweenFrames;

    [SerializeField]
    private Sprite[] _sprites;

    int index;
    float timer;

    private void Start()
    {
        _renderer.sprite = _sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < _timeBetweenFrames) return;

        index++;
        index %= _sprites.Length;

        _renderer.sprite = _sprites[index];

        timer = 0.0f;
    }
}
