using UnityEngine;
using TMPro;
using System.Drawing;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int currentScore = 0;
    [SerializeField] int pointsPerSecond;
    private int highScore = 0;

    [SerializeField] TMP_Text textPrefab;
    [SerializeField] Transform textCanvas;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    int point;


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

        point = pointsPerSecond / 60;
    }

    private void FixedUpdate()
    {
        currentScore += point;
        UpdateScoreUI();
    }

    public void AddScore(int value, Vector2 textPos)
    {
        currentScore += value;

        TMP_Text text = Instantiate(textPrefab, textPos, Quaternion.identity, textCanvas);
        text.text = "+" + value;

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
