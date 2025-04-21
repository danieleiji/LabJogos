using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    #region Public Variables
    public int CurrentScore { get; private set; }
    public int PlayerLives { get; private set; }
    #endregion


    #region Private Variables
    [SerializeField] private Text _livesText; //Pode usar Text, ou TextMeshProUGUI, como preferir
    [SerializeField] private Text _scoreText; //Pode usar Text, ou TextMeshProUGUI
    private playerController _player;

    #endregion

    #region Unity Methods
    void Start()
    {
        //Encontra o player
        GameObject playerGameObject = GameObject.Find("Player");
        if (playerGameObject != null)
        {
            _player = playerGameObject.GetComponent<playerController>();
        }

        _livesText = GameObject.Find("Lives").GetComponent<Text>();
        _scoreText = GameObject.Find("Score").GetComponent<Text>();


        if (_livesText == null) Debug.LogError("Lives Text not found in Canvas");
        if (_scoreText == null) Debug.LogError("Score Text not found in Canvas");
        if (_player == null) Debug.LogError("Player GameObject not found");
    }


    void Update()
    {
        if (_player != null)
        {
            PlayerLives = _player.playerLives;
        }
        UpdateUI();
    }

    #endregion

    #region Public Methods

    public void IncreaseScore(int amount)
    {
        CurrentScore += amount;
    }

    #endregion

    #region Private Methods

    private void UpdateUI()
    {
        if (_livesText != null) _livesText.text = "LIVES: " + PlayerLives;
        if (_scoreText != null) _scoreText.text = "SCORE: " + CurrentScore;
    }

    #endregion
}