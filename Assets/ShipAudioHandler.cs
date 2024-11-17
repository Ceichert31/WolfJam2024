using UnityEngine;

public class ShipAudioHandler : MonoBehaviour
{
    private Health health;

    [SerializeField]
    private AudioSource _source;

    [SerializeField]
    private AudioClip[] _deathSounds;

    [SerializeField]
    private AudioClip[] _damageSounds;

    private void Start()
    {
        health = GetComponent<Health>();

        health.OnDeath += PlayDeathSound;

        health.OnHealthUpdate += PlayDamageSound;
    }

    private void PlayDeathSound()
    {
        _source.PlayOneShot(_deathSounds[Random.Range(0, _deathSounds.Length - 1)]);
    }

    private void PlayDamageSound(int oldHealth, int newHealth)
    {
        // damage
        if(oldHealth > newHealth)
        {
            _source.PlayOneShot(_damageSounds[Random.Range(0, _deathSounds.Length - 1)]);
        }
        // heal
        else if(newHealth > oldHealth)
        {

        }

    }
}
