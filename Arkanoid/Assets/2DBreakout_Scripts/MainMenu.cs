using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text highScoreText;
    private string highScoreKey = "HighScore";

    void Start()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            if (highScoreText != null)
            {
                highScoreText.text = "High Score: " + PlayerPrefs.GetInt(highScoreKey).ToString();
            }
        }
        else
        {
            if (highScoreText != null)
            {
                highScoreText.text = "High Score: 0";
            }
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game Called");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Debug.Log("Cannot quit in WebGL.");
#else
        Application.Quit();
#endif
    }

    public void Historia()
    {
        // Carrega a cena chamada "LV4".
        SceneManager.LoadScene("LV4");
    }
}