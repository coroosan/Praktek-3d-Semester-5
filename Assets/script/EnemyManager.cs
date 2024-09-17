using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array prefab musuh yang akan di-spawn
    public Transform[] spawnPoint;   // Array titik spawn musuh
    public int maxEnemie = 10;       // Maksimal musuh yang bisa muncul di stage

    private int currentEnemyCount = 0; // Jumlah musuh yang sedang aktif di stage
    public int maxEnemies = 10; // Batas maksimal musuh yang dapat di-spawn
    public GameObject fixedEnemyPrefab; // Prefab musuh yang akan di-spawn (tidak acak)
    public Transform[] spawnPoints; // Titik-titik spawn musuh

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
            // Pilih titik spawn secara acak
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            // Spawn musuh di titik yang dipilih dengan prefab musuh tetap
            Instantiate(fixedEnemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
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