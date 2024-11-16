using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 50;

    [SerializeField]
    private int _startingHealth = 50;

    [SerializeField]
    private bool _canTakeDamage = true;

    private int currentHealth;

    // getters
    public int CurrentHealth { get { return currentHealth; } }

    public delegate void HealthUpdate(int oldHealth, int newHealth);
    public HealthUpdate OnHealthUpdate;

    public delegate void Death();
    public Death OnDeath;

    private void Start()
    {
        // setup variables
        currentHealth = _startingHealth;
    }

    public void AddHealth(int healthAmount)
    {
        // ensure old health is set and update health accordingly
        int oldHealth = currentHealth;
        currentHealth += healthAmount;

        // ensure health does not surpass maximum
        if(currentHealth > _maxHealth) {
            currentHealth = _maxHealth;
        }

        OnHealthUpdate?.Invoke(oldHealth, currentHealth);
    }

    public bool TakeDamage(int healthAmount)
    {
        if (!_canTakeDamage) return false;

        int oldHealth = currentHealth;
        currentHealth -= healthAmount;

        // dead
        if (currentHealth <= 0) OnDeath?.Invoke();

        // take damage normally
        else OnHealthUpdate?.Invoke(oldHealth, currentHealth);

        return true;
    }

    public void RestoreHealth()
    {
        currentHealth = _maxHealth;
    }
}
