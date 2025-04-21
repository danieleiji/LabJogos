using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 1. Adicionar a namespace do TextMeshPro

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText; // Legacy UI Text para moedas

        // 2. Referência pública para o texto de morte (TextMeshPro)
        public TextMeshProUGUI deathMessageText;

        void Start()
        {
            // Tenta encontrar o jogador (melhor usar FindGameObjectWithTag se possível)
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null) {
                player = playerObj.GetComponent<PlayerController>();
            } else {
                 Debug.LogError("GameManager: Objeto 'Player' não encontrado!");
                 // Considerar desabilitar o GameManager ou tratar o erro
            }

            // 3. Garantir que o texto de morte comece desativado (caso tenha esquecido no Inspector)
            if (deathMessageText != null)
            {
                deathMessageText.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("GameManager: O campo 'Death Message Text' não foi atribuído no Inspector!");
            }
        }

        void Update()
        {
            // Atualiza o texto das moedas (usando o Text legado)
            if (coinText != null)
            {
                coinText.text = coinsCounter.ToString();
            }

            // Verifica se o jogador existe e se o estado de morte foi ativado
            if (player != null && player.deathState == true)
            {
                // --- Lógica de Morte ---
                playerGameObject.SetActive(false); // Desativa o jogador original

                // Instancia o prefab de morte na posição/rotação do jogador original
                GameObject deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                // Ajusta a escala do prefab de morte para corresponder à do jogador (se necessário)
                deathPlayer.transform.localScale = playerGameObject.transform.localScale; // Mais direto

                // 4. Ativar o texto "Você Morreu"
                if (deathMessageText != null)
                {
                    deathMessageText.gameObject.SetActive(true);
                }

                // Reseta o estado de morte no script do jogador para evitar que este bloco rode múltiplas vezes
                player.deathState = false;

                // Agenda o reinício do nível após 3 segundos
                Invoke("ReloadLevel", 3);
            }
        }

        private void ReloadLevel()
        {
            // Nota: Application.LoadLevel está obsoleto. Use SceneManager.
            // Application.LoadLevel(Application.loadedLevel); // Obsoleto

            // Forma moderna de recarregar a cena atual:
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}