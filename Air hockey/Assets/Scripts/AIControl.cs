using UnityEngine;

public class AIControl : MonoBehaviour
{
    public float speed = 8f;
    private Transform ballTransform;
    private Rigidbody2D rb2d;

    // Limites de movimento da IA (ajuste conforme necessário)
    private float minX = -4.6f;
    private float maxX = 4.7f;
    private float minY = 0f;  // Limita para a metade superior do campo
    private float maxY = 6.65f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D não encontrado em AIControl.");
            enabled = false;
            return;
        }

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            ballTransform = ball.transform;
        }
        else
        {
            Debug.LogError("Bola não encontrada! Verifique a tag 'Ball'.");
            enabled = false;
            return;
        }
    }

    void FixedUpdate()
    {
        if (ballTransform == null || rb2d == null)
        {
            return;
        }

        // 1. Calcula a direção para a bola.
        Vector2 directionToBall = ballTransform.position - transform.position;

        // 2.  Move a IA em direção à bola.
        Vector2 desiredMovement = directionToBall.normalized * speed * Time.fixedDeltaTime;

        // 3. Aplica o movimento, limitando a posição da IA.
        Vector2 newPosition = (Vector2)transform.position + desiredMovement;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rb2d.MovePosition(newPosition);


        // 4. (Opcional) Rebatida "automática" se a bola estiver perto.  Pode remover se quiser.
        if (Vector2.Distance(transform.position, ballTransform.position) < 1.0f)
        {
            //Aplica um pequeno impulso extra na bola, baseando-se na direção do movimento.
           Rigidbody2D ballRb = ballTransform.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                ballRb.AddForce(directionToBall.normalized * 2f, ForceMode2D.Impulse);  // Adiciona um impulso
            }

        }
    }
}