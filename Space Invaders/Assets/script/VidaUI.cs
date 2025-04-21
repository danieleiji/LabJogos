using UnityEngine;
using TMPro; // Importante!

public class VidaUI : MonoBehaviour
{
    public TextMeshProUGUI vidaText; // Usamos TextMeshProUGUI para UI Canvas
    private int vidaAtual = 3;

    //Dentro do seu Script VidaUI.cs
    public int GetVidaAtual()
    {
        return vidaAtual;
    }

    void Start()
    {
        AtualizarVidaText(); // Inicializa o texto
    }

    public void DiminuirVida()
    {
        vidaAtual--;
        if (vidaAtual < 0)
        {
            vidaAtual = 0; // Evita vida negativa
        }
        AtualizarVidaText();
    }

    void AtualizarVidaText()
    {
        vidaText.text = "Vida: " + vidaAtual;
    }

   //Exemplo de uso de uma função que aumenta a vida:

   public void AumentarVida(int quantidade)
    {
        vidaAtual += quantidade;
        AtualizarVidaText();
    }
}