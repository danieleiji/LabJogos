using UnityEngine;

public class BossController : MonoBehaviour
{
    public int hitsToDestroy = 10;
    public AudioClip hitSound;       // Arraste o som de "hit" aqui no Inspector.
    public AudioClip destroySound;   // Arraste o som de destruição aqui no Inspector.
    public int pontos = 100; //Pontos ganhos ao destruir o boss

    private int currentHits = 0;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Adiciona um AudioSource dinamicamente.
        if (audioSource == null)
        {
             Debug.LogError("Erro: AudioSource não pode ser criado no Boss.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            currentHits++;

            if (audioSource != null && hitSound != null) // Toca o som de hit
            {
               audioSource.PlayOneShot(hitSound);
            }

            if (currentHits >= hitsToDestroy)
            {
                DestroyBoss();
            }
        }
    }

    private void DestroyBoss()
    {
        if (audioSource != null && destroySound != null)// Toca o som de destruição.
        {
            audioSource.PlayOneShot(destroySound);
        }

        Bloco.score += pontos; //Adiciona ao score usando o script já existente
        if (Bloco.scoreText != null)
        {
            Bloco.scoreText.text = "Score: " + Bloco.score;
        }
        else
        {
            Debug.LogWarning("ScoreText não foi atribuído!");
        }

        Destroy(gameObject); // Destrói o Boss *depois* de tocar o som.
    }
}