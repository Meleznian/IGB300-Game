using UnityEngine;

public class Launcher : OneWayPlatform
{    
    [SerializeField] float launchForce;
    [SerializeField] Transform aimer;
    //[SerializeField] Vector3 launchDirection;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null)
        {
            player.GetComponent<Rigidbody2D>().AddForce((aimer.transform.up.normalized)*launchForce,ForceMode2D.Impulse);
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawWireCube(transform.position, new Vector3());
    //}

}
