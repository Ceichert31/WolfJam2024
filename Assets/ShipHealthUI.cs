using UnityEngine;
using UnityEngine.UI;

public class ShipHealthUI : MonoBehaviour
{
    [SerializeField]
    private UnitManager _myUnitManager;

    [SerializeField]
    private Health _health;

    [SerializeField]
    private Slider _healthUISlider;

    [SerializeField]
    private Slider _healthWhiteSlider;

    private void Start()
    {
        _health.OnHealthUpdate += HealthUpdated;
        _health.OnDeath += Death;
    }

    private void HealthUpdated(int oldhealth, int newhealth)
    {
        _healthUISlider.value = ((float)newhealth) / ((float)_health.MaxHealth);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
