using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public static int EnemiesAlive = 0;

    public Wave[] waves;

    public Transform spawnPoint;

    public float waveInterval = 5f;
    private float countdown = 30f;

    public Text waveCountdownText;

    private int waveIndex = 0;
    
    void Update ()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = waveInterval;
        }else if (EnemiesAlive > 0)
        {
            return;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave ()
    {
        PlayerStats.Rounds++;
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        waveIndex++;

        if(waveIndex == waves.Length)
        {
            Debug.Log("Level Complete");
            this.enabled = false;
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        EnemiesAlive++;
    }
}
