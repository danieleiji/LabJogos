using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroPlayer : MonoBehaviour
{

    private Rigidbody2D rb2d;
    public float speed = 5.0f;
    private PontuacaoUI pontuacaoUI;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //Encontra o script de pontuação
        VidaUI vidaUI = FindObjectOfType<VidaUI>();
        if (vidaUI != null)
        {
            pontuacaoUI = vidaUI.GetComponent<PontuacaoUI>();
        }
        if (pontuacaoUI == null)
        {
             Debug.LogError("Não encontrei o script PontuacaoUI no objeto!");
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Inimigo_Linha1")
        {
            if(pontuacaoUI != null){
                pontuacaoUI.AumentarPontuacao(10); // Adiciona 10 pontos por exemplo
            }
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "Inimigo_Linha2")
        {
            if(pontuacaoUI != null){
                pontuacaoUI.AumentarPontuacao(20); // Adiciona 20 pontos por exemplo
            }
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "Inimigo_Linha3")
        {
             if(pontuacaoUI != null){
                pontuacaoUI.AumentarPontuacao(30); // Adiciona 30 pontos por exemplo
             }
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "Inimigo_Linha4")
        {
            if(pontuacaoUI != null){
                pontuacaoUI.AumentarPontuacao(40); // Adiciona 40 pontos
            }
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "Tiro_Inimigo")
        {
            Destroy(coll.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var vel = rb2d.velocity;
        vel.y = speed;
        vel.x = 0;
        rb2d.velocity = vel;
    }
}