using UnityEngine;
using TMPro;
using EasyTextEffects;
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
    public TextMeshProUGUI deathScoreText;
    public TextMeshProUGUI deathHighScoreText;
    [SerializeField] TMP_Text newHighScore;
    [SerializeField] ParticleSystem scoreEffect;
    int point;
    private bool isAlive = true;
    private TextEffect effect;

    [SerializeField] int multIncrease;
    [SerializeField] float increaseAmount;
    int increaseBy;

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
        increaseBy = multIncrease;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;  // The score should be stop counting if dead
        currentScore += point;
        UpdateScoreUI();
    }

    public void AddScore(int value, Vector2 textPos)
    {
        currentScore += value;

        if(currentScore >= multIncrease)
        {
            GameManager.instance.cashMult += increaseAmount;
            multIncrease += increaseBy;

            EnemyManager.instance.IncreaseDifficulty();
        } 

        //TMP_Text text = Instantiate(textPrefab, textPos, Quaternion.identity, textCanvas);
        //text.text = "+" + value;

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
        if (deathScoreText != null)
        {
            if (currentScore >= highScore)
            {
                deathScoreText.text = "<link=red+shake>Score: " + currentScore.ToString() + "</link>";
                newHighScore.gameObject.SetActive(true);
                scoreEffect.Play();
            }
            else
            {

                deathScoreText.text = "<link=red+shake>Score: " + currentScore.ToString() + "</link>";

            }
        }
        deathHighScoreText.text = "<link=blue+shake>High Score: " + highScore.ToString() + "</link>";
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public void StopScoring()
    {
        isAlive = false;
        UpdateScoreUI();
    }

    private void Update()
    {
        ResetScore();
    }

    void ResetScore()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }
}