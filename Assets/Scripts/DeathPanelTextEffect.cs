using UnityEngine;
using TMPro;
using EasyTextEffects;

public class deathScoreEffect : MonoBehaviour
{
    public TMP_Text scoreText;
    private TextEffect effect;

    private void Start()
    {
        effect = scoreText.GetComponent<TextEffect>();
        scoreText.text = "Score: 1234";

        // Play some flashy effects
        effect.Refresh();
        effect.StartManualEffect("red");  
        effect.StartManualEffect("shake");    
    }
}
