using UnityEngine;

namespace Platformer
{
    public class EnemyAI : MonoBehaviour
    {
        public float moveSpeed = 2f;
        public float detectionRadius = 5f;
        public float stoppingDistance = 0.5f;
        public LayerMask ground;
        public LayerMask wall;

        private Rigidbody2D rigidbody;
        public Collider2D triggerCollider;
        private Transform playerTransform;
        private bool isFacingRight = true;
        private bool isChasing = false;

        // Referência para o Animator
        private Animator animator;
        // Nome do parâmetro booleano no Animator Controller
        private readonly int isMovingHash = Animator.StringToHash("isMoving"); // Mais eficiente que usar string toda hora

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>(); // Pega o componente Animator

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("EnemyAI: Não foi possível encontrar o objeto do jogador com a tag 'Player'.");
                this.enabled = false; // Desabilita o script se não achar o jogador
            }

            if (transform.localScale.x < 0)
            {
                isFacingRight = false;
            }
        }

        void Update()
        {
            if (playerTransform == null)
            {
                StopMoving();
                UpdateAnimation(false); // Garante que a animação pare
                return;
            }

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            bool shouldChase = distanceToPlayer <= detectionRadius;
            bool isCurrentlyMoving = false; // Flag para saber se definimos velocidade > 0 neste frame

            if (shouldChase)
            {
                isChasing = true;
                float directionToPlayerX = playerTransform.position.x - transform.position.x;

                if (directionToPlayerX > 0.1f && !isFacingRight) // Adiciona uma pequena margem para evitar viradas rápidas
                {
                    Flip();
                }
                else if (directionToPlayerX < -0.1f && isFacingRight)
                {
                    Flip();
                }

                if (Mathf.Abs(directionToPlayerX) > stoppingDistance)
                {
                    // Verifica antes se há uma parede ou borda IMEDIATAMENTE à frente
                    // (Isso é uma verificação extra, a do FixedUpdate é a principal)
                    bool willHitWall = triggerCollider.IsTouchingLayers(wall);
                    bool willFall = !triggerCollider.IsTouchingLayers(ground);

                    // Só move se NÃO for bater em parede ou cair IMEDIATAMENTE
                    if (!willHitWall && !willFall)
                    {
                       float targetHorizontalSpeed = isFacingRight ? moveSpeed : -moveSpeed;
                       rigidbody.velocity = new Vector2(targetHorizontalSpeed, rigidbody.velocity.y);
                       isCurrentlyMoving = true; // Marcamos que está tentando mover
                    }
                    else
                    {
                        StopMoving(); // Para se a verificação imediata falhar
                    }
                }
                else
                {
                    StopMoving(); // Para se estiver perto
                }
            }
            else // Jogador fora do raio
            {
                if (isChasing) // Se estava perseguindo antes, para
                {
                   isChasing = false;
                   StopMoving();
                }
                // Se não estava perseguindo, já deve estar parado, não faz nada.
            }

            UpdateAnimation(isCurrentlyMoving && isChasing); // Atualiza animação baseado no estado
        }

        void FixedUpdate()
        {
             // --- Detecção de Borda e Parede (Baseada na Física) ---
             // Só verifica se está tentando se mover (velocidade definida no Update)
             // Usamos uma pequena tolerância para evitar problemas com floats
             if (Mathf.Abs(rigidbody.velocity.x) > 0.01f)
             {
                bool approachingEdge = !triggerCollider.IsTouchingLayers(ground);
                bool approachingWall = triggerCollider.IsTouchingLayers(wall);

                 if (approachingEdge || approachingWall)
                 {
                     // Para imediatamente para não cair ou atravessar
                     StopMoving();
                     UpdateAnimation(false); // Atualiza animação para parado

                     // O Flip aqui pode ser opcional ou causar comportamento estranho
                     // se o jogador estiver do outro lado da parede/precipício.
                     // Considere se realmente quer que ele vire ao bater.
                     // Flip();
                 }
             }
        }

        private void StopMoving()
        {
            // Só altera se a velocidade X não for zero, para evitar chamadas desnecessárias
            if (rigidbody.velocity.x != 0)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
        }

         // Atualiza o parâmetro do Animator
        private void UpdateAnimation(bool moving)
        {
            if (animator != null)
            {
                animator.SetBool(isMovingHash, moving);
            }
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;

            // Importante: Se o seu triggerCollider for um filho do objeto principal,
            // ele NÃO vira automaticamente com a escala. Você pode precisar ajustar
            // a posição local dele ou ter dois triggers (um para cada lado).
            // Se o triggerCollider for no mesmo objeto, a escala deve virá-lo.
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stoppingDistance);
        }
    }
}