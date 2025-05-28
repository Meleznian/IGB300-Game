using UnityEngine;

public class BotCollected : MonoBehaviour
{
    public bool isbotCount = false; 



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bot")
        {
            
                Debug.Log("Bot collected"); 
                FindFirstObjectByType<GameManager>().BotCount();

            //collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            //Debug.Log("Destroy it");
            Destroy(collision.gameObject);
        }
    }
}
