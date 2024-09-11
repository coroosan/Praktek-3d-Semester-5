using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array prefab musuh yang akan di-spawn
    public Transform[] spawnPoints;   // Array titik spawn musuh
    public int maxEnemies = 10;       // Maksimal musuh yang bisa muncul di stage

    private int currentEnemyCount = 0; // Jumlah musuh yang sedang aktif di stage

    void Start()
    {
        // Memulai spawning musuh
        InvokeRepeating("TrySpawnEnemy", 1f, 2f); // Cek untuk spawn musuh setiap 2 detik
    }

    void TrySpawnEnemy()
    {
        // Cek jika jumlah musuh yang tersisa kurang dari 2 dari batas maksimal
        if (currentEnemyCount < maxEnemies - 2)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount < maxEnemies)
        {
            // Pilih prefab musuh secara acak
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            // Pilih titik spawn secara acak
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            // Spawn musuh di titik yang dipilih dengan prefab yang dipilih
            Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
            // Tambahkan jumlah musuh
            currentEnemyCount++;
        }
    }

    public void EnemyDefeated()
    {
        // Kurangi jumlah musuh ketika musuh dikalahkan
        currentEnemyCount--;
    }
}
