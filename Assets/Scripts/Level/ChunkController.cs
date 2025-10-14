using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private int _poolIndex;
    [SerializeField] private float _despawnX = 30f;
    [SerializeField] Transform player;
    [SerializeField] float dist;
    //[SerializeField] private GameObject _killWall;
    //[SerializeField] private GameObject _thisChunk;

    private void Start()
    {
        player = GameManager.instance.Player.transform;
    }

    private void Update()
    {
        dist = Vector2.Distance(transform.position, player.position);
        print(dist );

        if (dist > _despawnX && transform.position.x < player.transform.position.x)
        {
            ChunkPoolManager.Instance.ReturnChunkToPool(gameObject, _poolIndex);
        }

    }
}
