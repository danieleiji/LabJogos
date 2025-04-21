using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float initialForceMagnitude = 25f;
    public float maxSpeed = 10f;
    public GameManager gameManager; // Referência para o GameManager
    private AudioSource audioSource;

    void GoBall()
    {
        float angle = Random.Range(20f, 70f);
        if (Random.value < 0.5f)
        {
            angle = -angle;
        }
        float xDirection = (Random.value < 0.5f) ? -1f : 1f;

        Vector2 force = new Vector2(
            initialForceMagnitude * xDirection * Mathf.Cos(angle * Mathf.Deg2Rad),
            initialForceMagnitude * Mathf.Sin(angle * Mathf.Deg2Rad)
        );
        rb2d.AddForce(force);
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D não encontrado em BallControl.");
            enabled = false;
            return;
        }

        // Encontrar o GameManager no início (IMPORTANTE e CORRIGIDO)
        gameManager = FindFirstObjectByType<GameManager>(); // Mudança aqui!
        if (gameManager == null)
        {
            Debug.LogError("GameManager não encontrado na cena!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource não encontrado no objeto BallControl.");
        }

        rb2d.linearVelocity = Vector2.zero;
        Invoke("GoBall", 2); // Atraso inicial
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Toca o som, verificando primeiro se audioSource existe.
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("PlayerAI"))
        {
            Debug.Log("Colisão com Player/IA");
            float y = (rb2d.linearVelocity.y / 2f) + (collision.collider.attachedRigidbody.linearVelocity.y / 2f);
            Vector2 newVelocity = new Vector2(rb2d.linearVelocity.x, y);
            rb2d.linearVelocity = newVelocity;
            rb2d.linearVelocity = Vector2.ClampMagnitude(rb2d.linearVelocity, maxSpeed);
        }
        else if (collision.gameObject.name == "PontoAcima" || collision.gameObject.name == "PontoAbaixo")
        {
            Debug.Log("COLISÃO DETECTADA: " + collision.gameObject.name);
            if (gameManager != null)
            {
                gameManager.Score(collision.gameObject.name);
            }
        }
    }

    public void ResetBall(float x, float y)
    {
        rb2d.linearVelocity = Vector2.zero;
        transform.position = new Vector2(x, y);
        Invoke("GoBall", 1); // Atraso antes de reiniciar
    }
}