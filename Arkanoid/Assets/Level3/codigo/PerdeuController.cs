using UnityEngine;

public class PerdeuController : MonoBehaviour
{
    public GerenciaJogo gameManager; // Referência ao GameManager.  Arraste o objeto GerenciaJogo para cá no Inspector!

    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("Erro: GerenciaJogo não atribuído ao PerdeuController! Arraste o objeto GerenciaJogo para o Inspector.");
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            gameManager.GameOver(); // Chama a função GameOver do GameManager.
        }
    }
}