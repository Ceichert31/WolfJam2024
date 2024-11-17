using UnityEngine;

public class DespawnParticle : MonoBehaviour
{
    [SerializeField]
    private float _time = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(Cleanup), _time);
    }

    // Update is called once per frame
    void Cleanup()
    {
        Destroy(gameObject);
    }
}
