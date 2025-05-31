using UnityEngine;
using UnityEngine.Events;

public class GenericRelay : MonoBehaviour
{
    [SerializeField] UnityEvent functionToRelay1;
    [SerializeField] UnityEvent functionToRelay2;
    [SerializeField] UnityEvent functionToRelay3;


    public void CallFunction1()
    {
        functionToRelay1.Invoke();
    }
    public void CallFunction2()
    {
        functionToRelay2.Invoke();
    }
    public void CallFunction3()
    {
        functionToRelay3.Invoke();
    }

}
