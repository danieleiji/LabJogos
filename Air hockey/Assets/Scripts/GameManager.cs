using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;
    public GUISkin layout;
    private GameObject theBall;

    // Posição do botão no MUNDO DO JOGO.
    public Vector2 resetButtonWorldPosition = new Vector2(10, 5);

    // Tamanho do botão (ainda em pixels de TELA).
    public Vector2 buttonSize = new Vector2(120, 40);

    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
        if (theBall == null)
        {
            Debug.LogError("Bola não encontrada! Verifique a tag 'Ball'.");
        }
    }

    public void Score(string wallID)
    {
        if (wallID == "PontoAbaixo")
        {
            PlayerScore2++;
            if (theBall != null)
            {
                theBall.GetComponent<BallControl>().ResetBall(0, -2);
            }
        }
        else if (wallID == "PontoAcima")
        {
            PlayerScore1++;
            if (theBall != null)
            {
                theBall.GetComponent<BallControl>().ResetBall(0, 2);
            }
        }
        Debug.Log("Placar: Jogador Red: " + PlayerScore1 + " | PlayerAI: " + PlayerScore2);
    }

    public void ResetGame()
    {
        PlayerScore1 = 0;
        PlayerScore2 = 0;

        if (theBall != null)
        {
            theBall.GetComponent<BallControl>().ResetBall(0, 0);
        }
    }

    void OnGUI()
    {
        if (layout != null)
        {
            GUI.skin = layout;
        }

        GUI.Label(new Rect(Screen.width / 2 - 300 - 12, 20, 100, 100), "" + PlayerScore1);
        GUI.Label(new Rect(Screen.width / 2 + 300 - 12, 20, 100, 100), "" + PlayerScore2);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = Color.red;
        style.fontSize = 50;
        style.alignment = TextAnchor.MiddleCenter;

        if (PlayerScore1 >= 10)
        {
            GUI.Label(new Rect(Screen.width / 2 - 300, 200, 600, 100), "PLAYER RED WINS", style);
            if (theBall != null)
            {
                theBall.GetComponent<BallControl>().ResetBall(0, -2);
            }
        }
        else if (PlayerScore2 >= 10)
        {
            GUI.Label(new Rect(Screen.width / 2 - 300, 200, 600, 100), "PLAYER AI WINS", style);
            if (theBall != null)
            {
                theBall.GetComponent<BallControl>().ResetBall(0, 2);
            }
        }

        // 1. Converter a posição do mundo para a tela.  IMPORTANTE!
        Vector2 screenPos = Camera.main.WorldToScreenPoint(resetButtonWorldPosition);

        // 2. Ajustar para que o ponto de referência do botão seja o centro, e não o canto superior esquerdo.
        screenPos.x -= buttonSize.x / 2;
        screenPos.y -= buttonSize.y / 2;

        //3. Inverter a coordenada Y, pois o y da tela cresce *para baixo*, e o y do mundo, para cima.
        screenPos.y = Screen.height - screenPos.y;


        // Desenhar o botão usando as coordenadas de TELA convertidas.
        if (GUI.Button(new Rect(screenPos.x, screenPos.y, buttonSize.x, buttonSize.y), "Reset"))
        {
            ResetGame();
        }
    }
}