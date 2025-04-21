using UnityEngine;
using TMPro;

public class Bloco : MonoBehaviour
{
    public int pontos = 10;  // Pontos por destruir este bloco.
    public static int score = 0; // Pontuação total (estática - compartilhada).
    public static TextMeshProUGUI scoreText; // Referência estática ao texto do placar.

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            AumentarScore();
            Destroy(gameObject); // Destrói o bloco.
        }
    }

    private void AumentarScore()
    {
        score += pontos; // Aumenta a pontuação.

        // Atualiza o texto do placar *somente* se a referência for válida.
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("ScoreText não foi atribuído! Verifique o script GerenciaJogo.");
        }
    }
}