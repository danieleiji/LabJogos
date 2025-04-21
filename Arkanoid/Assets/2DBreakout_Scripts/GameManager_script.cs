using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_script : MonoBehaviour
{
    public GameObject gameOverPanel;
    public static bool gameOver = false;
    public string nextLevelName = "LV2"; //  Vai ser alterado dinamicamente.
    public string menuSceneName = "Menu";
    public KeyCode cheatKey = KeyCode.F12;

    private int totalBricks;
    private int destroyedBricks = 0;

    public Text scoreText;
    public Text highScoreText;  // Certifique-se de ter um Text para o HighScore no seu Canvas.
    private int currentScore = 0;
    private string highScoreKey = "HighScore";
    private int highScore;  // Armazena o high score carregado.


    void Start()
    {
        totalBricks = GameObject.FindGameObjectsWithTag("Brick").Length;
        destroyedBricks = 0;
        UpdateScoreText();
        LoadHighScore();  // Carrega o high score no início.

        // Define o nextLevelName com base na cena atual.
        if (SceneManager.GetActiveScene().name == "LV1")
        {
            nextLevelName = "LV2";
        }
        else if (SceneManager.GetActiveScene().name == "LV2")
        {
            nextLevelName = "LV3";
        }
        // Adicione mais else if se você tiver mais níveis.
        else
        {
            nextLevelName = menuSceneName; // Volta para o menu.
        }
    }

    void Update()
    {
        // Evita lógica de jogo no menu.
        if (SceneManager.GetActiveScene().name.Equals(menuSceneName))
        {
            return;
        }

        if (Input.GetKeyDown(cheatKey))
        {
            LoadNextLevel();
        }

        if (Input.GetButtonDown("Jump") && gameOver == true)
        {
            gameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarrega a cena atual.
        }
    }

    public void BrickDestroyed(int brickScore)
    {
        destroyedBricks++;
        currentScore += brickScore;
        UpdateScoreText();
        CheckForNewHighScore();

        if (destroyedBricks >= totalBricks)
        {
            LoadNextLevel();
        }
    }


    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogWarning("Next Level Name not set!");
        }

    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            //  String interpolation.
            scoreText.text = $"Score: {currentScore}";
        }
    }

    private void LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
             // Carrega o high score corretamente.
            highScore = PlayerPrefs.GetInt(highScoreKey);
            if (highScoreText != null)
            {
                highScoreText.text = $"HighScore: {highScore}";
            }
        }
    }


    private void CheckForNewHighScore()
    {
        if (currentScore > PlayerPrefs.GetInt(highScoreKey, 0)) // 0 é o valor padrão se a chave não existir.
        {
            PlayerPrefs.SetInt(highScoreKey, currentScore);
            PlayerPrefs.Save(); //  Importante salvar!
            highScore = currentScore;   // Atualiza a variável highScore.
            if(highScoreText != null)   // Atualiza o texto na tela se houver um novo record.
            {
                highScoreText.text = $"HighScore: {highScore}";
            }
        }
    }
}