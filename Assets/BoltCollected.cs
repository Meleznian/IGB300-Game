using UnityEngine;

public class BoltCollected : MonoBehaviour
{
    //public bool isboltCount = false;
    private int _value;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collided with: " + collision.gameObject.name);

        Debug.Log($"[COLLISION] GameObject: {collision.gameObject.name}, Collider: {collision.collider.name}, Contact count: {collision.contactCount}");

        if (collision.gameObject.tag == "BrassBolt")
        {
            _value = 5;//divid by two 
        }

        if (collision.gameObject.tag == "GoldBolt")
        {
            _value = 12;//divid by two 
        }

        if (LayerMask.LayerToName(collision.gameObject.layer) == "Bolt")
        {
            Debug.Log("value " + _value);
            Debug.Log("Bolt collected");
            GameManager.instance.BoltCount(_value);
            //GameManager.instance.BoltCount(collision.gameObject.GetComponent<MagnetBolt>().value);

            //collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            //Debug.Log("Destroy it");
            Destroy(collision.gameObject);
            
        }
    }
}
