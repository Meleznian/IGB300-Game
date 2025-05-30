using UnityEngine;

public class BoltCollected : MonoBehaviour
{
    public bool isboltCount = false; 

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bolt")
        {
                Debug.Log("Bolt collected"); 
                GameManager.instance.BoltCount(collision.gameObject.GetComponent<MagnetBolt>().value);

            //collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            //Debug.Log("Destroy it");
            Destroy(collision.gameObject);
            
        }
    }
}
