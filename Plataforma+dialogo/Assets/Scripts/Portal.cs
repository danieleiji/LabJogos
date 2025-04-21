using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para carregar cenas

namespace Platformer // << Garanta que está no namespace correto
{
    [RequireComponent(typeof(Collider2D))] // Garante que há um Collider2D
    public class Portal : MonoBehaviour
    {
        [Tooltip("O nome exato da cena a ser carregada.")]
        public string sceneToLoad = "LV2"; // Verifique se este é o nome correto da sua cena

        [Tooltip("Tag do objeto que pode ativar este portal (normalmente 'Player').")]
        public string requiredTag = "Player";

        private DialogueTrigger dialogueTrigger; // Referência ao gatilho de diálogo neste portal
        private bool playerIsInside = false;    // O jogador está atualmente dentro do trigger?
        private bool isLoadingScene = false;    // Previne múltiplas tentativas de carregar a cena

        void Awake()
        {
            // Tenta encontrar o componente DialogueTrigger no mesmo GameObject
            dialogueTrigger = GetComponent<DialogueTrigger>();
            // Se não encontrar, o portal funcionará sem diálogo (carregamento imediato)
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isLoadingScene) return; // Se já está carregando, não faz nada

            if (other.CompareTag(requiredTag))
            {
                Debug.Log($"Portal: '{requiredTag}' entrou no trigger de '{gameObject.name}'.");
                playerIsInside = true;

                // CASO 1: Existe um DialogueTrigger neste portal
                if (dialogueTrigger != null && dialogueTrigger.dialogue != null)
                {
                    // Verifica se o DialogueManager existe
                    if (DialogueManager.Instance != null)
                    {
                         // Só inicia o diálogo se ele ainda não foi iniciado OU se não for 'triggerOnce'
                         // E se o diálogo específico não estiver já rodando
                        if ((!dialogueTrigger.triggerOnce || !GetTriggerHasBeenTriggered(dialogueTrigger)) &&
                            !DialogueManager.Instance.IsDialogueActive(dialogueTrigger.dialogue))
                        {
                            Debug.Log($"Portal: Iniciando diálogo '{dialogueTrigger.dialogue.name}' e esperando o fim.");
                            // Assina o método para carregar a cena QUANDO o diálogo terminar
                            DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;
                            dialogueTrigger.TriggerDialogue(); // Inicia o diálogo
                        }
                        else if (DialogueManager.Instance.IsDialogueActive(dialogueTrigger.dialogue))
                        {
                             Debug.Log($"Portal: Diálogo '{dialogueTrigger.dialogue.name}' já está ativo. Esperando o fim.");
                             // Mesmo se já estiver ativo, garante que vamos ouvir o fim dele
                             DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;
                        }
                        else
                        {
                             Debug.Log($"Portal: Diálogo '{dialogueTrigger.dialogue.name}' já foi ativado (triggerOnce=true). Carregando cena imediatamente.");
                             // Se for triggerOnce e já foi ativado, carrega direto
                             LoadScene();
                        }
                    }
                    else
                    {
                        Debug.LogError($"Portal: DialogueTrigger encontrado, mas DialogueManager não existe na cena! Carregando cena imediatamente.");
                        LoadScene(); // Carrega sem diálogo como fallback
                    }
                }
                // CASO 2: NÃO existe DialogueTrigger neste portal
                else
                {
                    Debug.Log($"Portal: Sem DialogueTrigger configurado em '{gameObject.name}'. Carregando cena imediatamente.");
                    LoadScene(); // Carrega a cena direto
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isLoadingScene) return;

            if (other.CompareTag(requiredTag))
            {
                Debug.Log($"Portal: '{requiredTag}' saiu do trigger de '{gameObject.name}'.");
                playerIsInside = false;

                // Se o jogador sair ANTES do diálogo terminar, remove a assinatura
                // para NÃO carregar a cena quando o diálogo acabar.
                if (dialogueTrigger != null && DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;
                    Debug.Log($"Portal: Assinatura de HandleDialogueEnd removida porque o jogador saiu.");
                }
            }
        }

        // Método chamado pelo evento OnDialogueEnd do DialogueManager
        private void HandleDialogueEnd()
        {
            // Remove a assinatura imediatamente para evitar chamadas múltiplas
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;
            }

            // Só carrega a cena se o jogador AINDA estiver dentro do trigger E não estivermos já carregando
            if (playerIsInside && !isLoadingScene)
            {
                Debug.Log($"Portal: Diálogo terminou e jogador está dentro. Iniciando carregamento da cena.");
                LoadScene();
            }
            else if (!playerIsInside)
            {
                Debug.Log($"Portal: Diálogo terminou, mas o jogador saiu antes. Cena não será carregada.");
            }
        }

        // Carrega a cena especificada
        private void LoadScene()
        {
            if (isLoadingScene) return; // Previne execução dupla

            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                isLoadingScene = true; // Marca que estamos carregando
                Debug.Log($"Portal: Carregando cena '{sceneToLoad}'...");
                // Opcional: Adicionar um fade out aqui antes de carregar
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError($"Portal '{gameObject.name}': 'Scene To Load' não está definida!", this);
            }
        }

        // Método auxiliar para verificar o estado 'hasTriggered' privado do DialogueTrigger
        // (Idealmente, DialogueTrigger teria uma propriedade pública para isso, mas isso funciona)
        private bool GetTriggerHasBeenTriggered(DialogueTrigger trigger)
        {
             // Usa reflexão para acessar o campo privado. Não é ideal, mas evita modificar DialogueTrigger agora.
             System.Reflection.FieldInfo field = typeof(DialogueTrigger).GetField("hasTriggered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
             if (field != null)
             {
                 return (bool)field.GetValue(trigger);
             }
             Debug.LogWarning("Não foi possível acessar hasTriggered de DialogueTrigger via reflexão.");
             return false; // Supõe que não foi ativado se não conseguir ler
        }


        // Limpeza caso o portal seja destruído
        void OnDestroy()
        {
             if (DialogueManager.Instance != null)
             {
                 DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;
             }
        }
    }
} // << IMPORTANTE: Feche o namespace