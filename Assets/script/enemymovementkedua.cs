using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemymovementkedua : MonoBehaviour
{
    public Transform player;             // Referensi ke transform pemain
    public NavMeshAgent enemy;           // Referensi ke NavMeshAgent pada musuh
    public float shootingRange = 10f;    // Jarak tembak musuh
    public float stoppingDistance = 5f;  // Jarak berhenti musuh
    public GameObject bulletPrefab;      // Prefab peluru
    public Transform bulletSpawnPoint;   // Titik spawn peluru
    public float fireRate = 1f;          // Kecepatan tembak
    public float bulletSpeed = 20f;      // Kecepatan peluru

    private bool isShooting = false;     // Apakah musuh sedang menembak

    void Update()
    {
        // Menghitung jarak ke pemain
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Kunci posisi Y agar tidak berubah
        Vector3 fixedPosition = transform.position;
        fixedPosition.y = 0;
        transform.position = fixedPosition;

        // Jika jarak ke pemain dalam jarak tembak
        if (distanceToPlayer <= shootingRange)
        {
            // Berhenti mendekati pemain dan siap menembak
            enemy.stoppingDistance = stoppingDistance;
            enemy.SetDestination(transform.position); // Berhenti di tempat

            // Menghadap ke pemain
            FacePlayer();

            // Jika belum menembak, mulai proses menembak
            if (!isShooting)
            {
                StartCoroutine(Shoot());
            }
        }
        // Jika jarak ke pemain lebih dari jarak tembak
        else
        {
            // Kejar pemain dan hentikan proses menembak
            enemy.stoppingDistance = 0f; // Tidak ada jarak berhenti saat mengejar
            enemy.SetDestination(player.position);
            StopShooting();              // Hentikan proses menembak
        }
    }

    // Fungsi untuk memutar musuh agar menghadap ke pemain
    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Kunci arah pada sumbu Y

        Quaternion lookRotation = Quaternion.LookRotation(direction); // Hanya rotasi di sumbu Y
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotasi
    }

    // Coroutine untuk menembak secara berkala
    IEnumerator Shoot()
    {
        isShooting = true;
        while (isShooting)
        {
            // Membuat peluru dan mengatur arah serta kecepatan peluru
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            // Menembak ke arah pemain
            if (rb != null)
            {
                Vector3 shootDirection = (player.position - bulletSpawnPoint.position).normalized;
                shootDirection.y = 0; // Kunci arah peluru pada sumbu Y
                rb.velocity = shootDirection * bulletSpeed;
            }

            // Tunggu sesuai kecepatan tembak
            yield return new WaitForSeconds(fireRate);
        }
    }

    // Fungsi untuk menghentikan proses menembak
    void StopShooting()
    {
        if (isShooting)
        {
            StopAllCoroutines();  // Hentikan semua coroutine
            isShooting = false;   // Reset status menembak
        }
    }
}
