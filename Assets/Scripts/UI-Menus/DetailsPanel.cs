using EasyTextEffects;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DetailsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text[] texts;
    [SerializeField] Image[] bolts;

    [SerializeField] float sumDelay;
    [SerializeField] float textDelay;

    int i = 0;
    int image = 0;
    int goal;
    int current;
    bool summing;
    [SerializeField] bool begun;
    TMP_Text currentText;
    float timer;

    // Update is called once per frame
    private void Start()
    {
        foreach( TMP_Text text in texts)
        {
            text.gameObject.SetActive(false);
        }
        foreach (Image im in bolts)
        {
            im.enabled = false;
        }
    }
    void Update()
    {
        if (begun)
        {
            if (summing)
            {
                SumNum();
            }
            timer += Time.deltaTime;
        }
    }

    public void DoText()
    {

        if (i < texts.Length)
        {
            texts[i].gameObject.SetActive(true);

            if (texts[i].GetComponent<TextEffect>() != null)
            {
                texts[i].GetComponent<TextEffect>().StartManualEffect("typewriter");
                i++;
            }
            else
            {
                StartSum(texts[i]);
                i++;
            }
        }
    }

    public void StartSum(TMP_Text text)
    {
        goal = GetGoal(text.gameObject.name);
        print("Current Goal is: " + goal);
        current = 0;
        currentText = text;
        summing = true;
    }

    void SumNum()
    {
        if (timer > sumDelay)
        {
            current += 1;
            currentText.text = current.ToString();

            if(currentText.name == "DistanceCount")
            {
                currentText.text = current.ToString() + "m";
            }

            timer = 0;
        }
        if(current == goal)
        { 
            summing = false;
            DoText();
        }
    }

    int GetGoal(string name)
    {
        print(name);
        if(name == "Steel_Bolt Text")
        {
            bolts[image].enabled = true;
            image += 1;
            return GameManager.instance.steelBolts;
        }
        else if (name == "Brass_Bolt Text")
        {
            bolts[image].enabled = true;
            image += 1;
            return GameManager.instance.brassBolts;
        }
        else if (name == "Silver_Bolt Text")
        {
            bolts[image].enabled = true;
            image += 1;
            return GameManager.instance.silverBolts;
        }
        else if (name == "Gold_Bolt Text")
        {
            bolts[image].enabled = true;
            image += 1;
            return GameManager.instance.goldBolts;
        }
        else if (name == "DistanceCount")
        {
            return (int)GameManager.instance.distanceTravelled;
        }
        else if (name == "Enemies killed")
        {
            return GameManager.instance._killCount;
        }
        else
        {
            return 0;
        }
    }

    public void BeginText()
    {
        begun = true;
        DoText();
    }
}
