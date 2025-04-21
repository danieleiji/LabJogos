using UnityEngine;

public class Perdeu : MonoBehaviour
{
    public GUISkin layout; // Opcional, para estilos customizados

    void OnGUI()
    {
        if (layout != null)
        {
            GUI.skin = layout;
        }

        // Estilos (pode ser movido para dentro do GUISkin)
        GUIStyle estiloTitulo = new GUIStyle(GUI.skin.label);
        estiloTitulo.fontSize = 40;
        estiloTitulo.normal.textColor = Color.red;
        estiloTitulo.alignment = TextAnchor.MiddleCenter; // Centraliza o texto

        GUIStyle estiloBotao = new GUIStyle(GUI.skin.button);
        estiloBotao.fontSize = 20;
        // ... mais estilos para o botão

        // Calcula a posição centralizada
        float larguraTela = Screen.width;
        float alturaTela = Screen.height;
        float larguraLabel = 500; // Ajuste conforme necessário
        float alturaLabel = 100;
        float larguraBotao = 200;
        float alturaBotao = 50;

        Rect retanguloLabel = new Rect(
            (larguraTela - larguraLabel) / 2,
            (alturaTela - alturaLabel) / 2 - 50, // Um pouco acima do centro
            larguraLabel,
            alturaLabel
        );

        Rect retanguloBotao = new Rect(
            (larguraTela - larguraBotao) / 2,
            (alturaTela - alturaBotao) / 2 + 50, // Um pouco abaixo do centro
            larguraBotao,
            alturaBotao
        );
        // Desenha os elementos
        GUI.Label(retanguloLabel, "Que pena! Você perdeu o jogo!", estiloTitulo);

        if (GUI.Button(retanguloBotao, "Restart Game", estiloBotao))
        {
             SceneChanger.Restart(); // ou SceneManager.LoadScene(...) como no exemplo anterior
        }
    }
}