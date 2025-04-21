using UnityEngine;

namespace Platformer // << IMPORTANTE: Adicione seu namespace
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Tooltip("O Asset de Diálogo que este gatilho deve iniciar.")]
        public Dialogue dialogue;

        [Tooltip("Marque se este diálogo deve iniciar automaticamente quando a cena carregar.")]
        public bool triggerOnStart = false;

        [Tooltip("Marque se este diálogo só pode ser ativado uma vez.")]
        public bool triggerOnce = true;

        private bool hasTriggered = false; // Controle interno para triggerOnce

        void Start()
        {
            // Se configurado para iniciar no começo e ainda não foi ativado
            if (triggerOnStart && (!triggerOnce || !hasTriggered))
            {
                // Pequeno atraso para garantir que o DialogueManager.Instance já existe
                Invoke(nameof(TriggerDialogueInternal), 0.1f);
            }
        }

        // Método público que pode ser chamado por outros scripts (ex: botão, evento)
        public void TriggerDialogue()
        {
             TriggerDialogueInternal();
        }

        // Método interno para iniciar o diálogo
        private void TriggerDialogueInternal()
        {
            // Verifica se já foi ativado (e só pode uma vez)
            if (triggerOnce && hasTriggered)
            {
                return; // Sai se já foi ativado e só pode uma vez
            }

            // Verifica se o DialogueManager existe
            if (DialogueManager.Instance != null)
            {
                // Verifica se o diálogo específico já está rodando (evita reiniciar o mesmo)
                if (!DialogueManager.Instance.IsDialogueActive(dialogue))
                {
                     Debug.Log($"Triggering dialogue: {dialogue.name} from {gameObject.name}");
                    DialogueManager.Instance.StartDialogue(dialogue);
                    hasTriggered = true; // Marca como ativado (para triggerOnce)
                }
            }
            else
            {
                Debug.LogError("DialogueTrigger: DialogueManager não encontrado na cena!");
            }
        }

        // Ativa o diálogo por colisão (usado pelo Portal)
        // Garante que o objeto tenha um Collider2D com 'Is Trigger' marcado
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Só ativa por colisão se NÃO for para ativar no Start
            // E verifica se quem colidiu tem a tag "Player" (ajuste se sua tag for diferente)
            if (!triggerOnStart && collision.CompareTag("Player"))
            {
                Debug.Log($"Player entered DialogueTrigger on {gameObject.name}");
                TriggerDialogueInternal(); // Tenta iniciar o diálogo
            }
        }
    }
} // << IMPORTANTE: Feche o namespace