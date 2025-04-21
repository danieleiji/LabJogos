using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax;
}

public class Paddle_script : MonoBehaviour
{
    public Boundary boundary;
    public float paddleSpeed;

    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        rb2D.velocity = movement * paddleSpeed;

        rb2D.position = new Vector2(Mathf.Clamp(rb2D.position.x, boundary.xMin, boundary.xMax), -4f);
    }
}