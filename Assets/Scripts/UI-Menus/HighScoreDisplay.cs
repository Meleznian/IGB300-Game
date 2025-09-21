using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
