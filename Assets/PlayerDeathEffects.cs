using UnityEngine;

public class PlayerDeathEffects : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();

        health.OnDeath += PlayDeathEffects;
    }

    void PlayDeathEffects()
    {
        EffectManager.instance.CallScreenFlash(0.5f);
        EffectManager.instance.CameraShake(1.0f, 2.0f, 30);
    }
}
