using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 targetDirection;

    private float speed = 1f;

    private Vector2 playerPosition => GameManager.Instance.Player.position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetDirection = (playerPosition - (Vector2)transform.position).normalized;
    }

    /// <summary>
    /// Sets the speed of a projectile (Default 1)
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed) => this.speed = speed;

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = targetDirection * speed;
    }
}
