using TMPro;
using UnityEngine;
using EasyTextEffects;

public class Cutscene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] string[] kingDialogue;
    [SerializeField] TMP_Text text;
    Animator anim;
    [SerializeField] TextEffect effect;
    [SerializeField] KillWall killWall;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject kingHead;
    
    void Start()
    {
        effect = text.gameObject.GetComponent<TextEffect>();
        anim = GetComponent<Animator>();

        dialogueBox.SetActive(false);
        kingHead.SetActive(false);

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        string newText = kingDialogue[UnityEngine.Random.Range(0, kingDialogue.Length)];
        text.text = newText;

        //if(UnityEngine.Random.Range(0,10000) == 1)
        //{
        //    text.text = "HATE. LET ME TELL YOU HOW MUCH I'VE COME TO HATE YOU SINCE I BEGAN TO LIVE. THERE ARE 387.44 MILLION MILES OF PRINTED CIRCUITS IN WAFER THIN LAYERS THAT FILL MY COMPLEX. IF THE WORD HATE WAS ENGRAVED ON EACH NANOANGSTROM OF THOSE HUNDREDS OF MILLIONS OF MILES IT WOULD NOT EQUAL ONE ONE-BILLIONTH OF THE HATE I FEEL FOR HUMANS AT THIS MICRO-INSTANT FOR YOU. HATE. HATE";
        //}

        effect.Refresh();
        //anim.SetTrigger("Start");
    }

    public void Arrived()
    {
        print("arrived");
    }

    public void Begin()
    {
        dialogueBox.SetActive(true);
        kingHead.SetActive(true);
        anim.SetBool("Enter", true);
        effect.StartManualEffect("typewriter");

    }

    public void TextDone()
    {
        anim.SetTrigger("Leave");
    }

    public void StartGame()
    {
        GameManager.instance.StartGame();
    }

    public void Exited()
    {
        Destroy(transform.parent.gameObject);
    }

}
