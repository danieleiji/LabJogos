using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed = 10f;
    private Camera mainCamera;
    public float yMax = 0f; // Limite máximo em Y (linha do meio do campo).

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D não encontrado em PlayerRed.");
            enabled = false;
        }
        if (mainCamera == null)
        {
            Debug.LogError("Câmera principal não encontrada.");
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(mousePos);
        worldMousePos.z = transform.position.z;

        // Limitando o movimento dentro da área da câmera E do lado do jogador (no eixo Y).
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(worldMousePos.x, mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x, mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x),
            Mathf.Clamp(worldMousePos.y, mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y, yMax) // Usando yMax aqui
        );
        rb2d.MovePosition(clampedPosition);
    }
}