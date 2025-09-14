using UnityEngine;
using TMPro;
using EasyTextEffects;
using System.Drawing;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections;

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
    //private bool isAlive = true;
    private TextEffect effect;
    private Vector3 baseScale;

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
        baseScale = scoreText.transform.localScale;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.playerDead) return;  // The score should be stop counting if dead
        currentScore += point;
        UpdateScoreUI();
    }


    public void AddScore(int value, Vector2 textPos)
    {
        currentScore += value;

        if (currentScore >= multIncrease)
        {
            GameManager.instance.cashMult += increaseAmount;
            multIncrease += increaseBy;
            EnemyManager.instance.IncreaseDifficulty();
        }

        // Spawn floating score text under canvas (works!)
        TMP_Text text = Instantiate(textPrefab, textPos, Quaternion.identity, textCanvas);
        text.text = "+" + value;

        // Hook up RisingText animation
        RisingText rising = text.GetComponent<RisingText>();
        if (rising != null)
        {
            // Fly to score UI text
            Vector2 scoreUIPos = scoreText.transform.position;
            rising.Init(scoreUIPos, () =>
            {
                UpdateScoreUI();
                StartCoroutine(BounceEffect());
            });
        }

        // Update high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        UpdateScoreUI();
    }



    public IEnumerator BounceEffect()
{
    Vector3 originalScale = baseScale;         
    Vector3 targetScale = baseScale * 1.2f;          


    float t = 0;
    while (t < 1)
    {
        t += Time.deltaTime * 8f;
        scoreText.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
        yield return null;
    }

    // Shrink back
    t = 0;
    while (t < 1)
    {
        t += Time.deltaTime * 8f;
        scoreText.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
        yield return null;
    }
}


    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + currentScore.ToString();
        highScoreText.text = "High Score: " + highScore.ToString();
        if (deathScoreText != null)
        {
            if (currentScore >= highScore)
            {
                deathScoreText.text = "Score: " + currentScore.ToString();
                //New High Score
                newHighScore.gameObject.SetActive(true);
                scoreEffect.Play();
            }
            else
            {

                deathScoreText.text = "Score: " + currentScore.ToString();

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
        //isAlive = false;
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