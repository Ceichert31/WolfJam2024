using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private List<int> destroyMask;

    [SerializeField] private List<int> damageMask;

    [SerializeField] private ParticleSystem _explosion;

    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    private ProjectileStats stats;

    private float waitTime;

    Vector2 finalTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Setting sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stats.BulletSprite;

        //Set lifetime duration
        waitTime = Time.time + stats.LifeTime;

        destroyMask.Clear();
        //Add enemy layer as destructable layer
        if (stats.EnemyLayer == 3)
        {
            destroyMask.Add(0);
            destroyMask.Add(stats.EnemyLayer);
            destroyMask.Add(3);
            damageMask.Add(stats.EnemyLayer);
        }
        else
        {
            destroyMask.Add(0);
            destroyMask.Add(stats.EnemyLayer);
            damageMask.Add(stats.EnemyLayer);
        }
    }

    /// <summary>
    /// Sets the speed of a projectile (Default 1)
    /// </summary>
    /// <param name="speed"></param>
    public void SetStats(ProjectileStats stats) => this.stats = stats;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, finalTarget * stats.Speed, Color.red);

        //Set velocity
        rb.linearVelocity = stats.Direction * stats.Speed;

        //Destroy projectile once lifetime is over
        if (waitTime <= Time.time)
            DestroyProjectile();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == stats.ParentObject) return;

        if (damageMask.Contains(collision.gameObject.layer))
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            collision.GetComponent<Unit>().MyUnitManager.MyHealth.TakeDamage(stats.Damage);
        }

        if (destroyMask.Contains(collision.gameObject.layer))
        {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        //Play effects
        Destroy(gameObject);
    }
}
