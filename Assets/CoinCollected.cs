using UnityEngine;

public class CoinCollected : MonoBehaviour
{
    public int currenctCoin = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.transform.name + " trigger it");

        if(other.gameObject.tag == "Coin")
        {
            //Debug.Log("Destroy it");
            currenctCoin += 100;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 1f);
            Debug.Log("Destroy it");

        }

    }


}
