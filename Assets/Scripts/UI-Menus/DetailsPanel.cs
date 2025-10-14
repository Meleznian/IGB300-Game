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

    [SerializeField] float sumSpeed;
    //[SerializeField] float textDelay;

    int i = 0;
    int image = 0;
    int goal;
    float current;
    bool summing;
    [SerializeField] bool begun;
    TMP_Text currentText;
    //float timer;
    //float speed;


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
            //timer += Time.deltaTime;
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
        //goal = 100;
        print("Current Goal is: " + goal);
        current = 0;
        currentText = text;
        currentText.text = current.ToString();
        summing = true;
    }

    void SumNum()
    {
        if (Mathf.Round(current) == goal)
        {
            print("Goal Reached");
            summing = false;
            DoText();
        }
        //else if (timer > sumSpeed)
        //{
            current = Mathf.Lerp(current, goal, sumSpeed*Time.deltaTime);
            
            currentText.text = Mathf.Round(current).ToString();

            if(currentText.name == "DistanceCount")
            {
                currentText.text += "m";
            }

        print(current);
            //timer = 0;
        //}

    }

    int GetGoal(string name)
    {
        print(name);
        if (name == "Steel_Bolt Text")
        {
            bolts[image].enabled = true;
            //image += 1; I have hidden the image generate 
            return GameManager.instance.steelBolts;
        }
        else if (name == "Brass_Bolt Text")
        {
            bolts[image].enabled = true;
            //image += 1;
            return GameManager.instance.brassBolts;
        }
        else if (name == "Silver_Bolt Text")
        {
            bolts[image].enabled = true;
            //image += 1;
            return GameManager.instance.silverBolts;
        }
        else if (name == "Gold_Bolt Text")
        {
            bolts[image].enabled = true;
            //image += 1;
            return GameManager.instance.goldBolts;
        }
        else if (name == "DistanceCount")
        {
            return (int)GameManager.instance.distanceTravelled;
        }
        else if (name == "KillCount")
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
