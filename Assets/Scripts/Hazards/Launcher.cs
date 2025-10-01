using UnityEngine;

public class Launcher : MonoBehaviour
{    
    [SerializeField] float launchForce;
    [SerializeField] Transform aimer;
    [SerializeField] Animator anim;
    //[SerializeField] Vector3 launchDirection;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null)
        {
            anim.SetTrigger("Activate");
            player.GetComponent<Rigidbody2D>().AddForce((aimer.transform.up.normalized)*launchForce,ForceMode2D.Impulse);
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawWireCube(transform.position, new Vector3());
    //}

}
