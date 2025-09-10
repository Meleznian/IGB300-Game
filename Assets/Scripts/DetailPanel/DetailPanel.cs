using UnityEngine;
using TMPro;

public class DetailPanel : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI distanceText;

    [Header("Bolt UI Array")]
    public TextMeshProUGUI[] boltTexts; // Steel, Brass, Silver, Gold

    private void Start()
    {
        // Clear UI at start
        UpdateKills(0);
        UpdateDistance(0);
        ClearBoltTexts();
    }

    public void UpdateKills(int kills)
    {
        if (killsText != null)
            killsText.text = "Enemies killed: " + kills;
    }

    public void UpdateDistance(float distance)
    {
        distanceText.text = "Distance travelled: " + distance.ToString("F1") + "m";
    }

    public void UpdateBoltCount(int index, int amount)
    {
        if (boltTexts != null && index >= 0 && index < boltTexts.Length)
            boltTexts[index].text = amount.ToString();
    }

    public void ClearBoltTexts()
    {
        if (boltTexts != null)
        {
            foreach (var txt in boltTexts)
            {
                if (txt != null)
                    txt.text = "0";
            }
        }
    }
}
