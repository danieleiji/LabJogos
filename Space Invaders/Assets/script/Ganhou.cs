using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ganhou : MonoBehaviour
{

    public GUISkin layout;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnGUI () {
        GUI.skin = layout;
        GUI.Label(new Rect(Screen.width / 2-100, 20, 1000, 1000), "Parabéns você ganhou o jogo!");

        
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 350, 100, 50), "Restart Game"))
        {
            SceneChanger.Restart();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
