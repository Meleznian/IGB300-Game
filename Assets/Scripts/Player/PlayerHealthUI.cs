using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] PlayerHealth playerHealth;

    void Start()
    {
        if (playerHealth != null && hpSlider != null)
        {
            hpSlider.maxValue = playerHealth.maxHealth;
            hpSlider.value = playerHealth.currentHealth;
        }
    }

    void Update()
    {
        if (playerHealth != null && hpSlider != null)
        {
            hpSlider.value = playerHealth.currentHealth;
        }
    }
}
