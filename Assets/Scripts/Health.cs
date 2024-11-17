using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 50;

    [SerializeField]
    private int _startingHealth = 50;

    [SerializeField]
    private bool _canTakeDamage = true;

    [SerializeField]
    private int currentHealth;

    [SerializeField] ParticleSystem smokeParticle;

    // getters
    public int CurrentHealth { get { return currentHealth; } }

    public int MaxHealth { get { return _maxHealth; } }

    public delegate void HealthUpdate(int oldHealth, int newHealth);
    public HealthUpdate OnHealthUpdate;

    public delegate void Death();
    public Death OnDeath;

    [SerializeField] GameObject DeathParticle;

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
        Debug.Log(healthAmount);

        int oldHealth = currentHealth;
        currentHealth -= healthAmount;

        EffectManager.instance.CameraShake(0.1f, 0.1f, 6);

        // dead
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            if (DeathParticle != null)
            {
                Instantiate(DeathParticle, transform.position, Quaternion.identity);
            }
            _canTakeDamage = false;
            EffectManager.instance.CameraShake(0.3f, 0.6f, 9);
        }

        // take damage normally
        else OnHealthUpdate?.Invoke(oldHealth, currentHealth);

        return true;
    }


    private void Update()
    {
        if (smokeParticle != null)
        {
            if (currentHealth < _maxHealth / 3)
            {
                if (!smokeParticle.isEmitting)
                {
                    smokeParticle.Play();

                }
            }
        }

    }
    public void RestoreHealth()
    {
        currentHealth = _maxHealth;
    }
}
