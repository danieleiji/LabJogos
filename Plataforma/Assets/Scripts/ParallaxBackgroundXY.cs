// ----- ParallaxBackgroundXY.cs -----
using UnityEngine;

namespace Platformer // Adicionado ao namespace
{
    public class ParallaxBackgroundXY : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("A Transform da câmera que o background deve seguir. Se vazio, tentará encontrar a Câmera Principal.")]
        public Transform cameraTransform;

        [Header("Parallax Effect")]
        [Tooltip("Fator de parallax horizontal. 0 = não move, 1 = move junto com a câmera.")]
        [Range(0f, 1f)]
        public float parallaxFactorX = 0.5f;

        [Tooltip("Fator de parallax vertical. 0 = não move, 1 = move junto com a câmera.")]
        [Range(0f, 1f)]
        public float parallaxFactorY = 0.3f;

        // Variáveis privadas
        private Vector3 startCameraPosition;    // Posição inicial da câmera
        private Vector3 startBackgroundPosition; // Posição inicial deste background
        private Transform backgroundTransform; // Cache da transform deste objeto

        void Start()
        {
            backgroundTransform = transform; // Cache

            // Encontra a câmera principal se não foi definida
            if (cameraTransform == null)
            {
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    cameraTransform = mainCamera.transform;
                     Debug.Log("ParallaxBackgroundXY: Câmera principal encontrada automaticamente.", this.gameObject);
                }
            }

            // Guarda posições iniciais se a câmera for válida
            if (cameraTransform != null)
            {
                startCameraPosition = cameraTransform.position;
                startBackgroundPosition = backgroundTransform.position;
            }
            else
            {
                Debug.LogError("ParallaxBackgroundXY: Nenhuma câmera encontrada ou definida no Inspector! O efeito parallax será desativado.", this.gameObject);
                enabled = false; // Desabilita o script
            }
        }

        // LateUpdate garante que o background se mova após a câmera
        void LateUpdate()
        {
            // Não faz nada se o script foi desabilitado ou a câmera não existe mais
            if (!enabled || cameraTransform == null) return;

            // Calcula o delta de movimento da câmera desde o início
            Vector3 cameraMovementDelta = cameraTransform.position - startCameraPosition;

            // Calcula a nova posição do background aplicando o fator de parallax
            float newPosX = startBackgroundPosition.x + (cameraMovementDelta.x * parallaxFactorX);
            float newPosY = startBackgroundPosition.y + (cameraMovementDelta.y * parallaxFactorY);

            // Aplica a nova posição, mantendo o Z original
            backgroundTransform.position = new Vector3(newPosX, newPosY, startBackgroundPosition.z);
        }
    }
}