using UnityEngine;

public class CurrencyHolder : MonoBehaviour
{
    [SerializeField] private int _totalCurrency;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCurrency(int currency)
    {
        _totalCurrency += currency;
    }
}
