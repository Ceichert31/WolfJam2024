using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float unitBounceForce;
    [SerializeField] float enemyBounceMultiplier;


    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Bounce off enemy ships
        Rigidbody2D enemyBody = collision.gameObject.GetComponent<Rigidbody2D>();
        Vector2 forceDir = (transform.position - collision.transform.position).normalized;
        float bounceForce = (body.linearVelocity * forceDir).magnitude * unitBounceForce;
        Debug.Log(body.linearVelocity);
        body.AddForce(forceDir * bounceForce, ForceMode2D.Impulse);
        enemyBody.AddForce(-forceDir * bounceForce * enemyBounceMultiplier, ForceMode2D.Impulse);
    }
    void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput).normalized;
        Vector2 targetVelocity = movementDirection * maxSpeed;
        body.linearVelocity = Vector2.Lerp(body.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }
}
