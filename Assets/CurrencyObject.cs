using UnityEngine;

public class CurrencyObject : MonoBehaviour, ICollectable
{
    public CurrencyHolder storage;
    public int value;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("Bounce");
        }
    }

    public void Collect()
    {
        //Perform Collection Process -- Number Go Up
        storage.AddCurrency(value);
        //Destroy
        Destroy(this.gameObject);
    }
}
