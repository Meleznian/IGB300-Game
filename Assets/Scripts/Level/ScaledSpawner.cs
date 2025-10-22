using UnityEngine;

public class ScaledSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnScaledBolt();
    }

    // Update is called once per frame
    void SpawnScaledBolt()
    {
        GameObject boltToSpawn = GameManager.instance.GetScaledBolt();
        GameObject bolt = Instantiate(boltToSpawn,transform.position, Quaternion.identity);

        bolt.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        Destroy(gameObject);
    }
}
