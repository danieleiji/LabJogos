using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 5f;
    public BallController ball; // Referência ao BallController *da bola*.
    private bool ballLaunched = false;


    void Start()
    {
        if (ball == null)
        {
            Debug.LogError("Erro: BallController não atribuído ao PaddleController! Arraste a *bola* para o Inspector.");
            return;
        }

    }

    void Update()
    {
        // Movimento do Paddle (horizontal).
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        // Limitação do movimento do Paddle (para não sair da tela).
        ClampPaddlePosition();

        // Lançamento da bola (tecla de espaço).
        if (Input.GetButtonDown("Jump") && !ballLaunched)  // "Jump" é o botão de espaço por padrão.
        {
            ball.Launch();  // Chama o método Launch() *do BallController*.
            ballLaunched = true; // Impede lançamentos múltiplos.
        }
    }

       
    public void ResetBallState()
    {
        ballLaunched = false;
    }


    private void ClampPaddlePosition()
    {
        // Obtém os limites da tela em coordenadas de mundo.
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        // Obtém a largura do paddle.
        float paddleWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        // Limita a posição X do paddle dentro dos limites da tela, considerando a largura do paddle.
        float clampedX = Mathf.Clamp(transform.position.x, minScreenBounds.x + paddleWidth / 2, maxScreenBounds.x - paddleWidth / 2);

        // Atualiza a posição do paddle.
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}