using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int destroyMask;

    private Rigidbody2D rb;

    private Vector2 targetDirection;

    private ProjectileStats stats;

    private float waitTime;

    private float spreadX;
    private float spreadY;
    private Vector2 playerPosition => GameManager.Instance.Player.position;

    Vector2 finalTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Calculate target direction
        //targetDirection = (playerPosition - (Vector2)transform.position).normalized;

        //Set lifetime duration
        waitTime = Time.time + stats.LifeTime;

        
        /*spreadX = Random.Range(0, 1);
        spreadY = Random.Range(0, 1);

        Vector2 spreadVec = new Vector2(spreadX, spreadY);

        targetDirection = (targetDirection + spreadVec).normalized;*/
    }

    /// <summary>
    /// Sets the speed of a projectile (Default 1)
    /// </summary>
    /// <param name="speed"></param>
    public void SetStats(ProjectileStats stats) => this.stats = stats;

    public void SetDirection(Vector2 direction)
    {
        targetDirection = direction;
        finalTarget = (targetDirection - (Vector2)transform.position).normalized;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, finalTarget * stats.Speed, Color.red);

        //Set velocity
        rb.linearVelocity = finalTarget * stats.Speed;

        //Set spread
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + spread));

        //Destroy projectile once lifetime is over
        if (waitTime <= Time.time)
            DestroyProjectile();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == destroyMask)
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
