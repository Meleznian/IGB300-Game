using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int currentScore = 0;
    private int highScore = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
    }

    public void AddScore(int value)
    {
        currentScore += value;

        // Update high score if needed
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + currentScore.ToString();
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }
}
