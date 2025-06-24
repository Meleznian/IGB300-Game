using UnityEngine;

namespace WaveManager
{
    public class WaveController : MonoBehaviour
    {
        [Header("References")]
        public TilemapWaveManager waveManager;
        public EnemySpawner enemySpawner; // Your enemy spawn system

        [Header("Settings")]
        public float timeBetweenWaves = 3f;

        private bool _waveActive;

        void Start() => StartNextWave();

        public void StartWave(int waveIndex, int enemiesToSpawn)
        {
            _waveActive = true;
            //enemySpawner.SpawnEnemies(enemiesToSpawn); // Implement your spawner
        }

        public void OnEnemyDefeated()
        {
            if (!_waveActive) return;

            // Check if all enemies are dead
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                WaveCompleted();
            }
        }

        void WaveCompleted()
        {
            _waveActive = false;
            Invoke(nameof(StartNextWave), timeBetweenWaves);
        }

        void StartNextWave() => waveManager.NextWave();
    }
}