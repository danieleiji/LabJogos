using UnityEngine;

namespace Platformer // << IMPORTANTE: Adicione seu namespace
{
    [System.Serializable]
    public class DialogueLine
    {
        [Tooltip("Sprite do personagem que está falando esta linha.")]
        public Sprite characterSprite;

        [Tooltip("A frase que será exibida.")]
        [TextArea(3, 10)] // Cria uma caixa de texto maior no Inspector
        public string sentence;
    }

    // Permite criar instâncias deste script como Assets no menu Create
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Platformer/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [Tooltip("A sequência de falas para esta conversa.")]
        public DialogueLine[] lines;
    }
} // << IMPORTANTE: Feche o namespace