using UnityEngine;
using System.Collections;

public class alienController : MonoBehaviour
{
    #region Public Variables
    public float enemySpeed = 0.1f;
    public GameObject alienBullet;  // Prefab da bala do alien
    public float bulletOffsetX = -0.1f; //Adicionado para poder editar o offset na unity
    public float bulletOffsetY = -0.02f; //Adicionado para poder editar o offset na unity
    #endregion

    #region Private Variables
    private float _timeLastShot;
    private int _bulletCounter;
    private bool _canShoot;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Renderer _renderer;
    private float _timeBetweenBullets = 0.5f; // Tempo entre tiros individuais
    private int _bulletsPerSeries = 3;     // Balas por rajada
    private float _timeBetweenShotsSeries;  // Tempo entre rajadas
    private float _timingShotSeries;
    #endregion

    #region Unity Methods
    void Awake()
    {
        // Armazena referências aos componentes para evitar chamadas repetidas a GetComponent
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<Renderer>();

        //Valida se os componentes foram encontrados
        if (_rb == null) Debug.LogError("Rigidbody2D not found on " + nameof(alienController));
        if (_collider == null) Debug.LogError("Collider2D not found on " + nameof(alienController));
        if (_renderer == null) Debug.LogError("Renderer not found on " + nameof(alienController));

        // Desativa o colisor inicialmente.  Será ativado quando o alien estiver visível.
        if (_collider != null) _collider.enabled = false;
    }


    void Start()
    {
        // Calcula o tempo entre rajadas.  +3 é um intervalo adicional.
        _timeBetweenShotsSeries = _bulletsPerSeries * _timeBetweenBullets + 3;

        // Posiciona o alien aleatoriamente dentro de limites específicos
        transform.position = new Vector3(Random.Range(2.45f, 4f), Random.Range(-1.70f, 0.44f), transform.position.z);
    }

    void Update() // Mudança de FixedUpdate para Update!
    {
        // Move o alien da direita para a esquerda, usando Time.deltaTime para consistência
        if (_rb != null) _rb.velocity = new Vector2(-enemySpeed, 0);

        // Lógica de tiro
        HandleShooting();
    }



    void OnBecameVisible()
    {
        // Se o alien tiver uma bala, ele pode atirar
        _canShoot = alienBullet != null;

        // Ativa o colisor quando o alien se torna visível
        if (_collider != null) _collider.enabled = true;
    }

    void OnBecameInvisible()
    {
        // Destroi o alien quando ele sai da tela.  Considerar pooling de objetos aqui no futuro!
        Destroy(gameObject);
    }

    //Usando OnCollisionEnter, os inimigos e o player precisam ter colliders, mas não marcados como trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        //Verifica se a colisão foi com um objeto na layer "Player"
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Destroi a bala e o player
            Destroy(other.gameObject); // Destroi o que colidiu com o alien (a bala)
            // Aumenta a pontuação
            if (Camera.main != null)
            {
                GUI gui = Camera.main.GetComponent<GUI>();
                if (gui != null)
                {
                    gui.IncreaseScore(1); // Agora temos um método para aumentar a pontuação!
                }
            }
            StartCoroutine(BlinkAndDestroy());
        }
    }

    

    #endregion

    #region Private Methods
    private void HandleShooting()
    {
        if (_canShoot && Time.time >= _timeLastShot && _bulletCounter < _bulletsPerSeries)
        {
            // Instancia a bala, usando pooling no futuro
            Instantiate(alienBullet, transform.position + new Vector3(bulletOffsetX, bulletOffsetY, 0), Quaternion.identity);
            _timeLastShot = Time.time + _timeBetweenBullets;
            _bulletCounter++;
        }

        // Reinicia o contador de balas após um intervalo
        if (Time.time >= _timingShotSeries)
        {
            _bulletCounter = 0;
            _timingShotSeries = Time.time + _timeBetweenShotsSeries;
        }
    }
    private IEnumerator BlinkAndDestroy()
    {
        // Faz o alien piscar antes de ser destruído.
        if (_renderer != null) _renderer.material.color = Color.cyan; //Muda para cor sólida

        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject); // Destrói o alien
    }

    #endregion
}