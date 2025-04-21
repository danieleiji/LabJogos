using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameControl : MonoBehaviour
{

    private float timer = 0.0f;
    private float waitTime = 30.0f;
    public NaveChefe nave;
    GameObject[] lin1;
    GameObject[] lin2;
    GameObject[] lin3;
    GameObject[] lin4;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        var posNave = transform.position;
        posNave.y = 3.5f;
        posNave.x = -8.0f;

        timer += Time.deltaTime;
        if (timer >= waitTime){
            Instantiate(this.nave, posNave, Quaternion.identity);
            timer = 0.0f;
        }

        lin1 = GameObject.FindGameObjectsWithTag("Inimigo_Linha1");
        lin2 = GameObject.FindGameObjectsWithTag("Inimigo_Linha2");
        lin3 = GameObject.FindGameObjectsWithTag("Inimigo_Linha3");
        lin4 = GameObject.FindGameObjectsWithTag("Inimigo_Linha4");
        foreach (GameObject i in lin4)
        {
            i.SendMessage("Atirar", null, SendMessageOptions.RequireReceiver);
        }
        if (!lin4.Any()) {
            foreach (GameObject i in lin3)
            {
                i.SendMessage("Atirar", null, SendMessageOptions.RequireReceiver);
            }
        } 
        if (!lin3.Any()) 
        {
            foreach (GameObject i in lin2)
            {
                i.SendMessage("Atirar", null, SendMessageOptions.RequireReceiver);
            }
        }
        if (!lin2.Any()) 
        {
            foreach (GameObject i in lin1)
            {
                i.SendMessage("Atirar", null, SendMessageOptions.RequireReceiver);
            }
        }
        if (!lin4.Any() && !lin3.Any() && !lin2.Any() && !lin1.Any()) {
            SceneChanger.Ganhou();
        }

    }
}
