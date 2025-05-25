using UnityEngine;

using UnityEngine.InputSystem;

public class Parry : MonoBehaviour
{

    InputAction parryAction;
    BehaviourAgent parryTarget;
    [SerializeField] Animator anim;
    bool parriable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parryAction = InputSystem.actions.FindAction("Parry");
    }

    // Update is called once per frame
    void Update()
    {
        if (parryAction.WasPressedThisFrame()) UseParry();
    }

    void UseParry()
    {
        anim.SetTrigger("Parry");
        StartCoroutine(parryTarget.Stunned());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<BehaviourAgent>(out parryTarget))
        {
            parriable = parryTarget.parriable;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (parryTarget == null) return;
        if(other.gameObject == parryTarget.gameObject) { 
            parryTarget = null;
        }
    }
}
