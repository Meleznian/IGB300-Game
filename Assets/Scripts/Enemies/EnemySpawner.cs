using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private float cooldown;
    [SerializeField] private float cooldownTimer;






    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(cooldownTimer >= cooldown)
        {
            cooldownTimer = 0;
            EnemyManager.instance.Spawn(transform, enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
        }
    }
}
