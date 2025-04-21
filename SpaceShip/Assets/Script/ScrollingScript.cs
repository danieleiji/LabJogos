using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollingScript : MonoBehaviour
{
    #region Public Variables
    public Vector2 speed = new Vector2(10, 10);   // Velocidade de rolagem
    public Vector2 direction = new Vector2(-1, 0); // Direção da rolagem
    public bool isLinkedToCamera = false;       // Se deve mover a câmera
    public bool isLooping = false;              // Se o fundo é infinito
    #endregion

    #region Private Variables
    private List<SpriteRenderer> _backgroundPart; // Lista das partes do fundo
    private Vector2 _repeatableSize;            // Tamanho do fundo que pode ser repetido
    #endregion


    #region Unity Methods
    void Start()
    {
        // Configuração para fundo infinito
        if (isLooping)
        {
            SetupInfiniteBackground();
        }
    }
    void Update()
    {
        // Movimento
        Vector3 movement = new Vector3(
            speed.x * direction.x,
            speed.y * direction.y,
            0) * Time.deltaTime; //Multiplica por deltaTime, importante

        transform.Translate(movement);

        // Move a câmera, se necessário
        if (isLinkedToCamera)
        {
            Camera.main.transform.Translate(movement);
        }

        // Repetição do fundo, se necessário
        if (isLooping)
        {
            LoopBackground();
        }
    }

    #endregion

    #region Private Methods

    private void SetupInfiniteBackground()
    {
        _backgroundPart = new List<SpriteRenderer>();

        // Adiciona todos os filhos com SpriteRenderer à lista
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            SpriteRenderer r = child.GetComponent<SpriteRenderer>();

            if (r != null)
            {
                _backgroundPart.Add(r);
            }
        }

        if (_backgroundPart.Count == 0)
        {
            Debug.LogError("Nothing to scroll!");
            return;
        }

        // Ordena as partes do fundo pela posição
        _backgroundPart = _backgroundPart.OrderBy(t => t.transform.position.x * (-1 * direction.x)).ThenBy(t => t.transform.position.y * (-1 * direction.y)).ToList();

        // Calcula o tamanho do fundo que pode ser repetido
        SpriteRenderer first = _backgroundPart.First();
        SpriteRenderer last = _backgroundPart.Last();

        _repeatableSize = new Vector2(
            Mathf.Abs(last.transform.position.x - first.transform.position.x),
            Mathf.Abs(last.transform.position.y - first.transform.position.y)
        );
    }
    private void LoopBackground()
    {
        // Obtém as bordas da câmera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        float dist = (transform.position - mainCamera.transform.position).z;
        float leftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        float rightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        float topBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        float bottomBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        // Determina as bordas de entrada e saída com base na direção
        Vector3 exitBorder = Vector3.zero;
        Vector3 entryBorder = Vector3.zero;

        if (direction.x < 0)
        {
            exitBorder.x = leftBorder;
            entryBorder.x = rightBorder;
        }
        else if (direction.x > 0)
        {
            exitBorder.x = rightBorder;
            entryBorder.x = leftBorder;
        }

        if (direction.y < 0)
        {
            exitBorder.y = bottomBorder;
            entryBorder.y = topBorder;
        }
        else if (direction.y > 0)
        {
            exitBorder.y = topBorder;
            entryBorder.y = bottomBorder;
        }

        // Obtém o primeiro filho
        SpriteRenderer firstChild = _backgroundPart.FirstOrDefault();

        if (firstChild != null)
        {
            bool checkVisible = false;

            // Verifica se o filho saiu da tela
            if (direction.x != 0)
            {
                if ((direction.x < 0 && (firstChild.transform.position.x < exitBorder.x))
                    || (direction.x > 0 && (firstChild.transform.position.x > exitBorder.x)))
                {
                    checkVisible = true;
                }
            }
            if (direction.y != 0)
            {
                if ((direction.y < 0 && (firstChild.transform.position.y < exitBorder.y))
                    || (direction.y > 0 && (firstChild.transform.position.y > exitBorder.y)))
                {
                    checkVisible = true;
                }
            }


            // Se o filho saiu da tela, desativa e substitui (pooling)
            if (checkVisible)
            {
                // DESATIVE o objeto
                firstChild.gameObject.SetActive(false);

                // Remova da lista ATUAL
                _backgroundPart.Remove(firstChild);

                // Encontre um objeto DESATIVADO (do pool) para reativar.  Se não houver, crie um novo.
                SpriteRenderer replacement = _backgroundPart.FirstOrDefault(sr => !sr.gameObject.activeSelf); // Procura um inativo
                if (replacement == null)
                {
                    // Se não encontrou um inativo, instancia um novo (isso deve acontecer raramente, depois da inicialização)
                    GameObject newGO = Instantiate(firstChild.gameObject); // Instancia a PARTIR do original (para manter configurações)
                    replacement = newGO.GetComponent<SpriteRenderer>();
                    _backgroundPart.Add(replacement); // Adiciona o NOVO à lista.
                }
                else
                {
                    //Remove da lista
                    _backgroundPart.Remove(replacement);
                }


                // POSICIONE o objeto de substituição (o que estava inativo, ou o novo)
                replacement.transform.position = new Vector3(
                    firstChild.transform.position.x + ((_repeatableSize.x + firstChild.bounds.size.x) * -1 * direction.x),
                    firstChild.transform.position.y + ((_repeatableSize.y + firstChild.bounds.size.y) * -1 * direction.y),
                    firstChild.transform.position.z
                );

                // REATIVE o objeto
                replacement.gameObject.SetActive(true);
                // Adicione-o ao final da lista.
                _backgroundPart.Add(replacement);
            }
        }
    }
    #endregion
}