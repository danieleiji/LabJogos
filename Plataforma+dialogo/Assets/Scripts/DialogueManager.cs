using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necessário para UI.Image
using TMPro; // Necessário para TextMeshProUGUI

namespace Platformer // << IMPORTANTE: Adicione seu namespace
{
    public class DialogueManager : MonoBehaviour
    {
        // Singleton para fácil acesso
        public static DialogueManager Instance { get; private set; }

        [Header("UI References")]
        [Tooltip("O painel principal da UI que contém todos os elementos do diálogo.")]
        public GameObject dialoguePanel;
        [Tooltip("O componente Image da UI onde o sprite do personagem será exibido.")]
        public Image characterImage;
        [Tooltip("O componente TextMeshProUGUI onde o texto do diálogo será exibido.")]
        public TextMeshProUGUI dialogueText;

        [Header("Dialogue Settings")]
        [Tooltip("Velocidade com que o texto aparece (segundos por caractere).")]
        public float typingSpeed = 0.03f;
        [Tooltip("Tecla para avançar o diálogo.")]
        public KeyCode continueKey = KeyCode.Space; // Ou KeyCode.E, etc.

        private Queue<DialogueLine> dialogueQueue; // Fila para processar as falas
        private bool isDialogueActive = false;    // O diálogo está sendo exibido?
        private Dialogue currentDialogue;         // Referência ao diálogo atual
        private Coroutine typingCoroutine;        // Referência à rotina de digitação

        // Evento chamado quando um diálogo termina completamente
        public System.Action OnDialogueEnd;

        void Awake()
        {
            // Configuração do Singleton
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Mais de um DialogueManager encontrado! Destruindo o novo.");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                dialogueQueue = new Queue<DialogueLine>();
                // Opcional: Mantenha o DialogueManager entre as cenas
                // DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            // Garante que a UI do diálogo começa desativada
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);
            else
                Debug.LogError("DialogueManager: Referência do Dialogue Panel não definida!");
        }

        void Update()
        {
            // Verifica se o diálogo está ativo e a tecla de continuar foi pressionada
            if (isDialogueActive && Input.GetKeyDown(continueKey))
            {
                // Se o texto ainda está sendo "digitado", completa-o imediatamente
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null; // Limpa a referência
                    // Garante que o texto completo da linha atual seja exibido
                     if(dialogueQueue.Count > 0 || (currentDialogue != null && currentDialogue.lines.Length > dialogueQueue.Count)) // Verifica se há uma linha sendo mostrada
                     {
                         // Cuidado: Precisamos da linha que *estava* sendo digitada.
                         // Como a fila já pode ter sido modificada, é mais seguro pegar a última linha teórica
                         // Esta lógica pode precisar de ajuste dependendo de como você quer o skip.
                         // Forma mais simples: apenas avança para a próxima.
                         // dialogueText.text = ???; // Difícil saber qual era a linha exata sem guardar.
                         // Ação mais segura: simplesmente avança para a próxima frase ou termina.
                         DisplayNextSentence();
                     }

                }
                else // Se o texto já está completo, avança para a próxima linha
                {
                    DisplayNextSentence();
                }
            }
        }

        // Método público para iniciar um novo diálogo
        public void StartDialogue(Dialogue dialogue)
        {
            if (isDialogueActive)
            {
                Debug.LogWarning("Tentativa de iniciar diálogo enquanto outro já está ativo.");
                return; // Não começa um novo se um já estiver rodando
            }
            if (dialogue == null || dialogue.lines.Length == 0)
            {
                 Debug.LogWarning("Tentativa de iniciar diálogo vazio ou nulo.");
                 return;
            }

            currentDialogue = dialogue; // Guarda a referência
            isDialogueActive = true;

            Debug.Log($"Iniciando diálogo: {dialogue.name}");

            // Limpa a fila de diálogos anteriores
            dialogueQueue.Clear();

            // Adiciona todas as linhas do diálogo atual à fila
            foreach (DialogueLine line in dialogue.lines)
            {
                dialogueQueue.Enqueue(line);
            }

            // Ativa o painel de diálogo na UI
            if (dialoguePanel != null) dialoguePanel.SetActive(true);

            // Opcional: Desabilitar controle do jogador ou pausar o jogo
            // PauseGame(true);

            // Exibe a primeira frase
            DisplayNextSentence();
        }

        // Exibe a próxima frase na fila
        public void DisplayNextSentence()
        {
            // Se a fila estiver vazia, termina o diálogo
            if (dialogueQueue.Count == 0)
            {
                EndDialogue();
                return;
            }

            // Retira a próxima linha da fila
            DialogueLine currentLine = dialogueQueue.Dequeue();

            // Atualiza a imagem do personagem (se houver)
            if (characterImage != null)
            {
                 characterImage.sprite = currentLine.characterSprite;
                 characterImage.enabled = (currentLine.characterSprite != null); // Mostra/esconde se tiver sprite
            }

            // Para qualquer corrotina de digitação anterior
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            // Inicia a corrotina para o efeito de digitação
            if (dialogueText != null)
                typingCoroutine = StartCoroutine(TypeSentence(currentLine.sentence));
            else
                 Debug.LogError("DialogueManager: Referência do Dialogue Text não definida!");
        }

        // Coroutine para exibir o texto letra por letra
        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = ""; // Limpa o texto anterior
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            typingCoroutine = null; // Marca que a digitação terminou
        }

        // Finaliza o diálogo atual
        void EndDialogue()
        {
            isDialogueActive = false;
            currentDialogue = null; // Limpa referência
            if(typingCoroutine != null) // Garante que parou a digitação
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            Debug.Log("Diálogo finalizado.");

            // Desativa o painel de diálogo na UI
            if (dialoguePanel != null) dialoguePanel.SetActive(false);

            // Opcional: Reabilitar controle do jogador ou despausar o jogo
            // PauseGame(false);

            // Dispara o evento para notificar outros scripts que o diálogo terminou
            OnDialogueEnd?.Invoke();
            // IMPORTANTE: Não limpar os assinantes aqui (OnDialogueEnd = null;)
            // pois o Portal precisa que seu assinante específico seja removido por ele mesmo.
        }

        // Método para verificar se um diálogo específico está ativo
        public bool IsDialogueActive(Dialogue dialogue)
        {
            return isDialogueActive && currentDialogue == dialogue;
        }

        // Método para verificar se *qualquer* diálogo está ativo
        public bool IsAnyDialogueActive()
        {
            return isDialogueActive;
        }

        // Opcional: Função helper para pausar/despausar
        // void PauseGame(bool pause)
        // {
        //     Time.timeScale = pause ? 0f : 1f;
        //     // Desabilitar/Habilitar script de controle do Player
        //     GameManager gm = FindObjectOfType<GameManager>(); // Precisa do seu GameManager
        //     if(gm != null && gm.playerGameObject != null)
        //     {
        //         PlayerController pc = gm.playerGameObject.GetComponent<PlayerController>();
        //         if (pc != null) pc.enabled = !pause;
        //     }
        // }
    }
}