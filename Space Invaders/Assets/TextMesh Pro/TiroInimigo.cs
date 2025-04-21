using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroInimigo : MonoBehaviour
{
    
    private Rigidbody2D rb2d;
    public float speed = -5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); 
    }

    void OnCollisionEnter2D (Collision2D coll) {
        if (coll.gameObject.tag == "Player")
        {
            // Destroy(coll.gameObject);
		}
        else if (coll.gameObject.tag == "Tiro_Player")
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
