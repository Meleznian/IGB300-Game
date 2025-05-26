using UnityEngine;

using UnityEngine.InputSystem;

public class Parry : MonoBehaviour
{

    InputAction parryAction;
    BehaviourAgent parryTarget;
    BoxCollider2D collider;
    [SerializeField] Animator anim;
    bool parriable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parryAction = InputSystem.actions.FindAction("Parry");
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (parryAction.WasPressedThisFrame()) UseParry();
    }

    void UseParry()
    {
        anim.SetTrigger("Parry");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<BehaviourAgent>(out parryTarget))
        {
            parriable = parryTarget.parriable;
            if (parryTarget != null && !parryTarget.stunned)
            {
                StartCoroutine(parryTarget.Stunned());
                EndParry();
            }
        }
    }

    //public void OnTriggerExit2D(Collider2D other)
    //{
    //    if (parryTarget == null) return;
    //    if(other.gameObject == parryTarget.gameObject) { 
    //        parryTarget = null;
    //    }
    //}

    internal void StartParry()
    {
        collider.enabled = true;
    }
    internal void EndParry()
    {
        if (collider.enabled == true)
        {
            collider.enabled = false;
        }
    }
}
