using UnityEngine;

public class BallController : MonoBehaviour
{
    public float initialSpeed = 600f;
    public float minSpeed = 400f;
    public float maxSpeed = 1000f;

    private Rigidbody2D rb;
    private bool ballInPlay = false;
    private Vector2 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) { Debug.LogError("Erro: Bola sem Rigidbody2D!"); return; }
        rb.isKinematic = true;
    }

    void Update()
    {
        if (ballInPlay)
        {
            lastVelocity = rb.velocity;
            ClampVelocity();
        }
    }

    public void Launch()
    {
        if (!ballInPlay)
        {
            transform.parent = null;
            ballInPlay = true;
            rb.isKinematic = false;

            // Ângulo aleatório entre 15 gfraus
            float randomAngle = Random.Range(-15f, 15f);

            // Converte o ângulo para um vetor de direção.
            Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;

            rb.AddForce(direction * initialSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ReflectBall(collision.contacts[0].normal);
    }

    private void ReflectBall(Vector2 normal)
    {
        Vector2 reflectedVelocity = Vector2.Reflect(lastVelocity.normalized, normal);
        rb.velocity = reflectedVelocity * lastVelocity.magnitude;
    }

    private void ClampVelocity()
    {
        float currentSpeed = rb.velocity.magnitude;

        if (currentSpeed < minSpeed)
        {
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
        else if (currentSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}