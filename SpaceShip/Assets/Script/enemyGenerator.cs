using UnityEngine;

public class enemyGenerator : MonoBehaviour
{
    #region Public Variables
    public GameObject alienPrefab;         // Prefab do alien a ser gerado
    public float spawnInterval = 2f;    // Intervalo entre a geração de aliens
    public float initialDelay = 1f;     // Atraso antes do primeiro alien aparecer
    #endregion

    #region Private Variables
    private float _nextSpawnTime;
    #endregion


    #region Unity Methods
    void Start()
    {
        // Define o tempo para o primeiro spawn
        _nextSpawnTime = Time.time + initialDelay;
    }

    void Update()
    {
        // Verifica se é hora de gerar um novo alien
        if (Time.time >= _nextSpawnTime)
        {
            SpawnAlien();
            _nextSpawnTime = Time.time + spawnInterval; // Agenda o próximo spawn
        }
    }
    #endregion


    #region Private Methods
    private void SpawnAlien()
    {
        // Instancia um novo alien. Usar pooling no futuro
        if (alienPrefab != null)
        {
            Instantiate(alienPrefab);
        }
        else
        {
            Debug.LogError("Alien Prefab not set on " + nameof(enemyGenerator));
        }
    }
    #endregion
}