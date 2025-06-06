using UnityEngine;

public class BoltCollected : MonoBehaviour
{
    //public bool isboltCount = false;
    //private float _value;

    [SerializeField] ParticleSystem collectEffect;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collided with: " + collision.gameObject.name);

        Debug.Log($"[COLLISION] GameObject: {collision.gameObject.name}, Collider: {collision.collider.name}, Contact count: {collision.contactCount}");

        //if (collision.gameObject.tag == "IronNut")
        //{
        //    _value = 0.5f;//one divide by two 
        //}
        //
        //if (collision.gameObject.tag == "SteelBolt")
        //{
        //    _value = 2.5f;//five divide by two 
        //}
        //
        //if (collision.gameObject.tag == "BrassBolt")
        //{
        //    _value = 5;//ten divide by two 
        //}
        //
        //if (collision.gameObject.tag == "SilverBolt")
        //{
        //    _value = 7.5f;//15 divide by two 
        //}
        //
        //if (collision.gameObject.tag == "GoldBolt")
        //{
        //    _value = 12.5f;//25 divide by two 
        //}

        if (LayerMask.LayerToName(collision.gameObject.layer) == "Bolt")
        {
            collectEffect.Play();
            //Debug.Log("value " + _value);
            Debug.Log("Bolt collected");
            //GameManager.instance.BoltCount(_value);
            GameManager.instance.BoltCount(collision.gameObject.GetComponent<MagnetBolt>().value);

            //collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            //Debug.Log("Destroy it");
            AudioManager.PlayEffect(SoundType.COLLECT_BOLT, 1f);
            Destroy(collision.gameObject);
            
        }
    }
}
