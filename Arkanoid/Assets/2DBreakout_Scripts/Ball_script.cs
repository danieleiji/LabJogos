using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball_script : MonoBehaviour
{
    public float ballInitialVelocity = 600f;
    public AudioClip sfxBrickHit;
    public AudioClip sfxPaddleBorderHit;

    private Rigidbody2D rb2D;
    private AudioSource audioSource;
    private bool ballInPlay;
    private GameManager_script gameManager;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager_script>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && ballInPlay == false)
        {
            transform.parent = null;
            ballInPlay = true;
            rb2D.isKinematic = false;

            if (Input.GetAxis("Horizontal") == 0f)
                rb2D.AddForce(new Vector2(1f, ballInitialVelocity));
            else if (Input.GetAxis("Horizontal") > 0f)
                rb2D.AddForce(new Vector2(ballInitialVelocity, ballInitialVelocity));
            else if (Input.GetAxis("Horizontal") < 0f)
                rb2D.AddForce(new Vector2(-ballInitialVelocity, ballInitialVelocity));
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Brick")
        {
            audioSource.clip = sfxBrickHit;
            audioSource.Play();

            // A linha importante foi corrigida aqui:
            gameManager.BrickDestroyed(coll.gameObject.GetComponent<Brick_script>().score);

            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "Paddle" || coll.gameObject.tag == "Border")
        {
            audioSource.clip = sfxPaddleBorderHit;
            audioSource.Play();
        }
    }
}