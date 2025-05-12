using UnityEngine;
using TMPro;

public class GerenciaJogo : MonoBehaviour // Use o nome CORRETO do seu script
{
    public TextMeshProUGUI scoreText; // Arraste o objeto ScoreText do Canvas AQUI.
    public TextMeshProUGUI gameOverText; // Arraste o objeto GameOverText do Canvas AQUI.
    public GameObject paddle;  // Arraste o objeto Paddle AQUI.
    public GameObject ball;    // Arraste o objeto Ball AQUI.
    public GameObject perdeuCollider;    // Arraste o objeto paredeperdeu AQUI.

    void Start()
    {
        // Inicializa o placar e a referência estática no script Bloco.
        if (scoreText != null)
        {
            Bloco.scoreText = scoreText;  // Define a referência estática (IMPORTANTE!).
            Bloco.score = 0; // Zera pontuação (opcional)
            scoreText.text = "Score: 0"; //Texto inicial (opcional)
        }
        else
        {
            Debug.LogError("Erro: ScoreText não atribuído em GerenciaJogo! Arraste-o do Canvas.");
        }

        //Verifica se gameOverText foi atribuido
        if (gameOverText == null)
        {
            Debug.LogError("Erro: GameOverText não atribuído em GerenciaJogo!  Arraste-o do Canvas.");
        }

        // Garante que o texto de Game Over comece desativado.
        if (gameOverText != null) // Checagem extra para evitar erros.
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true); // Ativa o texto.
        }

        //Pausa a bola
        ball.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().isKinematic = true;

        //Destroi os objetos que não vai mais utilizar, para poupar processamento.
        Destroy(paddle);
        Destroy(perdeuCollider);
    }
}