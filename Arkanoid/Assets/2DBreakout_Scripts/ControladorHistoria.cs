using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControladorHistoria : MonoBehaviour
{
    public GameObject historiaPanel; // Painel que contém o texto da história (atribuir no Inspector).
    public Text historiaText;       // Componente de texto para exibir a história (atribuir no Inspector).
    public float velocidadeTexto = 20f; // Velocidade da rolagem (ajustar no Inspector).
    public string textoHistoria;     // O texto completo da história (definir no Inspector).
    public KeyCode teclaFechar = KeyCode.Escape; //(Opcional) Tecla para fechar a história (definir no Inspector).

    private bool mostrandoHistoria = false;
    private RectTransform rectTransformTexto;

    void Start()
    {
        // Inicializa o painel como inativo e pega o RectTransform.
        if (historiaPanel != null)
        {
            historiaPanel.SetActive(false);
            rectTransformTexto = historiaText.GetComponent<RectTransform>();

            //Verifica se os objetos foram atribuidos.
            if (historiaText == null)
            {
                Debug.LogError("Erro: Componente Text não atribuído ao script ControladorHistoria!");
                enabled = false; //Desativa o script se houver erro.
                return;
            }

            if (historiaPanel == null)
            {
                Debug.LogError("Erro: GameObject do painel não atribuído ao script ControladorHistoria!");
                enabled = false; //Desativa o script se houver erro.
                return;
            }


        }

    }

    // Este Update() agora é opcional.  Só é necessário se você quiser usar a tecla para fechar.
    void Update()
    {
        // Fecha a história se a tecla configurada for pressionada.
        if (mostrandoHistoria && Input.GetKeyDown(teclaFechar))
        {
            FecharHistoria();
        }
    }


    // Método público para iniciar a exibição da história.  Chame este método do seu botão.
    public void MostrarHistoria()
    {
        if (!mostrandoHistoria && historiaPanel != null)
        {
            mostrandoHistoria = true;
            historiaPanel.SetActive(true);
            historiaText.text = textoHistoria;

            // Posiciona o texto no início.
            rectTransformTexto.anchoredPosition = new Vector2(0, -rectTransformTexto.rect.height / 2);

            StartCoroutine(RolarTexto());
        }
    }

    // Método público para fechar a história.
    public void FecharHistoria()
    {
        if (mostrandoHistoria)
        {
            mostrandoHistoria = false;
            StopAllCoroutines(); // Interrompe a rolagem.
            historiaPanel.SetActive(false);
        }
    }
    // Corrotina para animar a rolagem do texto.
    private IEnumerator RolarTexto()
    {
        float alturaPainel = historiaPanel.GetComponent<RectTransform>().rect.height;
        float alturaTexto = rectTransformTexto.rect.height;

        while (mostrandoHistoria && rectTransformTexto.anchoredPosition.y < alturaTexto + (alturaPainel / 2))
        {
            rectTransformTexto.anchoredPosition += new Vector2(0, velocidadeTexto * Time.deltaTime);
            yield return null; // Espera um frame
        }

        // Fecha a história automaticamente quando a rolagem termina.
        if (mostrandoHistoria)
        {
            FecharHistoria();
        }
    }
}