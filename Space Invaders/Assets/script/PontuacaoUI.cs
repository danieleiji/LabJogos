using UnityEngine;
using TMPro;

public class PontuacaoUI : MonoBehaviour
{
    public TextMeshProUGUI pontuacaoText; // Referência ao TextMeshPro
    private int pontuacaoAtual = 0;

    void Start()
    {
        AtualizarPontuacaoText(); // Inicializa o texto
    }

    public void AumentarPontuacao(int pontos)
    {
        pontuacaoAtual += pontos;
        AtualizarPontuacaoText();
    }

    void AtualizarPontuacaoText()
    {
        pontuacaoText.text = "Pontuação: " + pontuacaoAtual;
    }

    //Getter da pontuação
     public int GetPontuacaoAtual()
    {
        return pontuacaoAtual;
    }
}