using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode fire = KeyCode.Space;
    public float speed = 10.0f;
    public float boundX = 9f;
    Animator animator;
    private Rigidbody2D rb2d;
    public TiroPlayer tiro;
    public Transform pontoDeSaida;
    public float velocidadeDaBala = 10f; // Você não estava usando, mas mantive
    private int ultimoTempo = 0;
    private float timer = 0.0f;
    private float waitTime = 1.0f;

    private VidaUI vidaUI; // Referência para o script VidaUI

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        // tiro = GameObject.FindGameObjectWithTag("Tiro_Player"); // Esta linha não é mais necessária.

        vidaUI = FindObjectOfType<VidaUI>(); // Encontra o script VidaUI na cena.
        if (vidaUI == null)
        {
            Debug.LogError("Não encontrei o script VidaUI na cena!");
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Tiro_Inimigo")
        {
            // vida--; // REMOVIDO - A vida é diminuída no VidaUI
            if (vidaUI != null)
            {
                vidaUI.DiminuirVida(); // Chama a função do script VidaUI.
            }

            //Verifica se o VidaUI existe e se a vida é menor que 1.
            if (vidaUI != null && vidaUI.GetVidaAtual() < 1)  //Adicionado o método GetVidaAtual()
            {
                animator.SetTrigger("Active");
            }
            Destroy(coll.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        var vel = rb2d.velocity;
        if (Input.GetKey(moveRight))
        {
            vel.x = speed;
        }
        else if (Input.GetKey(moveLeft))
        {
            vel.x = -speed;
        }
        else
        {
            vel.x = 0;
        }
        rb2d.velocity = vel;

        var pos = transform.position;
        if (pos.x > boundX)
        {
            pos.x = boundX;
        }
        else if (pos.x < -boundX)
        {
            pos.x = -boundX;
        }
        transform.position = pos;


        var posTiro = transform.position;
        posTiro.y = pos.y + 0.2f;

        if (Input.GetKey(fire))
        {
            DateTime tempoAtual = DateTime.Now;
            if (tempoAtual.Second != ultimoTempo)
            {
                Instantiate(this.tiro, posTiro, Quaternion.identity);
                ultimoTempo = tempoAtual.Second;
            }
        }
        //Verifica se o vidaUI é nulo ANTES de tentar acessar um método
        if(vidaUI != null){
             if (vidaUI.GetVidaAtual() == 0)  //Adicionado método GetVidaAtual()
              {
                speed = 0;
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    SceneChanger.Perdeu();
                    timer = 0.0f;
                }
              }
        }

    }
}