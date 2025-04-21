using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 30f;
    public int bulletDirection = -1; // -1 para a esquerda (inimigo atirando para a esquerda)

    private Rigidbody2D _rb;

    void Awake()  // Use Awake para obter componentes
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado no prefab da bala!");
        }
    }

    void FixedUpdate() // Use FixedUpdate para física consistente
    {
        if (_rb != null)
        {
            _rb.velocity = new Vector2(bulletDirection * bulletSpeed, 0); // Define a velocidade diretamente
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}