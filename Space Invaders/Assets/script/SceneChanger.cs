using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;

    public GUISkin layout;
    GameObject theBall;
    GameObject bloco;
    string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SceneSet(string sceneName) {
        this.sceneName = sceneName;
    }

    public static void Ganhou(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Ganhou");
    }

    public static void Perdeu(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Perdeu");
    }

    
    public static void Restart(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Jogo");
    }



    // Update is called once per frame
    void Update()
    {
    }
}
