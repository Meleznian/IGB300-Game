using UnityEngine;

public class Springboard : MonoBehaviour
{
    [SerializeField] Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            anim.SetTrigger("Activate");
        }
    }

}
