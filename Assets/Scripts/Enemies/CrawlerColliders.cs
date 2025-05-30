using UnityEngine;

public class CrawlerColliders : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] Crawler parent;

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            parent.ChangeDirection();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parent.CrawlerAttack(other);
            AudioManager.PlayEffect(SoundType.CRAWLER_ATTACK);
        }
    }
}
