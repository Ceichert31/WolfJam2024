using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float stopDistance;

    [SerializeField] Rigidbody2D body;
    Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector2 movementDirection;

        if (Mathf.Abs((player.position - transform.position).magnitude) > stopDistance)
        {
            movementDirection = (player.position - transform.position).normalized;
        }
        else
        {
            movementDirection = Vector2.zero;
        }
        Vector2 targetVelocity = movementDirection * movementSpeed;
        body.linearVelocity = Vector2.Lerp(body.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }
}
