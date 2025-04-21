using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveControl : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float timer = 0.0f;
    private float waitTime = 1.0f;
    private float timer2 = 0.0f;
    private float waitTime2 = 1.5f;
    private int state = 0;
    private float x;
    Animator animator;
    public TiroInimigo tiro;
    public float boundX = 9f;
    public int atirar = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();   
        x = transform.position.x;
    }
    
    void ChangeState(){
        state = state + 1;
        if(state > 5){
            state = -5;
        }
    }

    void OnCollisionEnter2D (Collision2D coll) {
        if (coll.gameObject.tag == "Tiro_Player")
        {
            // animator.SetTrigger("Die");
            Destroy(coll.gameObject);
		}
	}

    void Atirar() {
        this.atirar = 1;
    }


    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        var posTiro = transform.position;
        if (atirar == 1){
            posTiro.y = pos.y - 0.2f;
            timer2 += Time.deltaTime;
            if (timer2 >= waitTime2){
                Instantiate(this.tiro, posTiro, Quaternion.identity);
                timer2 = 0.0f;
            }
        }

        timer += Time.deltaTime;
        if (timer >= waitTime){
            ChangeState();
            timer = 0.0f;
        }
        if(state >= -5 && state < 0){
            pos.x = x - state;
        }else if(state == 0){
            pos.y -= 0.5f;
            ChangeState();
            pos.x = x;
        }else if(state > 0 && state <=5){
            pos.x = x + state;
        }
        

        if (pos.x > boundX) {
            pos.x = boundX;
        }
        else if (pos.x < -boundX) {
            pos.x = -boundX;
        }
        transform.position = pos;
        

    }
}
