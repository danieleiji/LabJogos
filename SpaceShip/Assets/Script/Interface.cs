using UnityEngine;
using TMPro; // Importante: Namespace do TextMeshPro

public class Interface : MonoBehaviour
{
    #region Public Variables
    public int scoreToWin = 20; // Pontuação para vencer
    #endregion

    #region Private Variables
    [SerializeField] private TextMeshProUGUI _gameOverText; // Referência ao texto TMP de Game Over
    [SerializeField] private TextMeshProUGUI _victoryText;  // Referência ao texto TMP de Vitória
    private playerController _player;
    private GUI _gui;
    #endregion

    #region Unity Methods
    void Start()
    {
        //Encontra o Player e o GUI
        GameObject playerGameObject = GameObject.Find("Player");
        if(playerGameObject != null)
        {
            _player = playerGameObject.GetComponent<playerController>();
        }

        if(Camera.main != null)
        {
            _gui = Camera.main.GetComponent<GUI>();
        }

        _gameOverText = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        _victoryText = GameObject.Find("Victory").GetComponent<TextMeshProUGUI>();

        //Verificação
        if (_gameOverText == null) Debug.LogError("GameOver Text (TMP) not found!");
        if (_victoryText == null) Debug.LogError("Victory Text (TMP) not found!");
        if (_player == null) Debug.LogError("Player GameObject not found");
        if (_gui == null) Debug.LogError("GUI Script not found");

        // Desativa os textos no início
        if (_gameOverText != null) _gameOverText.gameObject.SetActive(false);
        if (_victoryText != null) _victoryText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckForGameOver();
        CheckForVictory();
    }

    #endregion


    #region Private Methods

    private void CheckForGameOver()
    {
        if (_player != null && _player.playerLives <= 0)
        {
            if (_gameOverText != null) _gameOverText.gameObject.SetActive(true); // Ativa o texto TMP
            _player.isGameOver = true; // Informa ao player
            Time.timeScale = 0;        // Pausa
        }
    }

    private void CheckForVictory()
    {
        if (_gui != null && _gui.CurrentScore >= scoreToWin)
        {
            if (_victoryText != null) _victoryText.gameObject.SetActive(true);  // Ativa o texto TMP
            _player.isGameOver = true; // Informa ao player
            Time.timeScale = 0;        // Pausa
        }
    }
    #endregion
}