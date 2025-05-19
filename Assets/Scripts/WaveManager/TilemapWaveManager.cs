using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveManager
{
    public class TilemapWaveManager : MonoBehaviour
    {
        [System.Serializable]
        public class WaveConfig
        {
            public string waveName;
            public Tilemap tilemap; // Assign in Inspector
            public int enemiesToSpawn = 5;
            public float delayBeforeStart = 0.5f;
        }

        [Header("Wave Settings")]
        public List<WaveConfig> waves;
        public float fadeDuration = 1f;

        private int currentWave = -1;
        private bool isTransitioning;

        void Start() => StartWave(0);

        public void StartWave(int waveIndex)
        {
            if (isTransitioning || waveIndex < 0 || waveIndex >= waves.Count) 
                return;

            StartCoroutine(TransitionToWave(waveIndex));
        }

        IEnumerator TransitionToWave(int newWaveIndex)
        {
            isTransitioning = true;

            // Fade out current wave
            if (currentWave >= 0)
            {
                Tilemap oldTilemap = waves[currentWave].tilemap;
                yield return FadeTilemap(oldTilemap, 1f, 0f);
                oldTilemap.gameObject.SetActive(false);
            }

            // Set up new wave
            WaveConfig newWave = waves[newWaveIndex];
            newWave.tilemap.gameObject.SetActive(true);
            yield return FadeTilemap(newWave.tilemap, 0f, 1f);

            // Wait before activation
            yield return new WaitForSeconds(newWave.delayBeforeStart);

            currentWave = newWaveIndex;
            isTransitioning = false;

            // Notify other systems (enemy spawner, etc.)
            FindFirstObjectByType<WaveController>()?.StartWave(newWaveIndex, newWave.enemiesToSpawn);
        }

        IEnumerator FadeTilemap(Tilemap tilemap, float startAlpha, float endAlpha)
        {
            float elapsed = 0f;
            TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
            Color originalColor = renderer.material.color;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                Color newColor = originalColor;
                newColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
                renderer.material.color = newColor;
                yield return null;
            }
        }

        public void NextWave() => StartWave(currentWave + 1);
    }
}