using UnityEngine;

public class TutorialExit : MonoBehaviour
{
    [SerializeField] GameObject killWall;
    [SerializeField] PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        killWall.SetActive(true);
        player.Die() ;
    }
}
