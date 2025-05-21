using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCollected : MonoBehaviour
{
    
    //public TextMeshProUGUI coinText;

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.transform.name + " trigger it");

        if(other.gameObject.tag == "Coin")
        {
            //Debug.Log("Destroy it");
            //call GameManager here
            FindFirstObjectByType<GameManager>().CoinCount();
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Debug.Log("Destroy it");
            Destroy(other.gameObject, 1f);
            

        }

    }


}
