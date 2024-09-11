using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;             // Referensi ke transform pemain
    public NavMeshAgent enemy;           // Referensi ke NavMeshAgent pada musuh
    public float shootingRange = 10f;    // Jarak tembak musuh
    public float stoppingDistance = 5f;  // Jarak berhenti musuh
    public GameObject bulletPrefab;      // Prefab peluru
    public Transform bulletSpawnPoint;   // Titik spawn peluru
    public float fireRate = 1f;          // Kecepatan tembak
    public float bulletSpeed = 20f;      // Kecepatan peluru
    public GameObject dangerImage;       // UI Image untuk logo danger
    public float warningDuration = 1.5f; // Durasi logo danger sebelum menembak

    private bool isShooting = false;     // Apakah musuh sedang menembak
    private bool isWarning = false;      // Apakah musuh sedang memberikan peringatan

    void Start()
    {
        // Memastikan logo danger tidak terlihat saat awal
        dangerImage.SetActive(false);
    }

    void Update()
    {
        // Menghitung jarak ke pemain
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Jika jarak ke pemain dalam jarak tembak
        if (distanceToPlayer <= shootingRange)
        {
            // Berhenti mendekati pemain dan siap menembak
            enemy.stoppingDistance = stoppingDistance;
            enemy.SetDestination(transform.position); // Berhenti di tempat

            // Jika belum menembak dan belum memberikan peringatan, mulai proses peringatan
            if (!isShooting && !isWarning)
            {
                StartCoroutine(ShowWarningThenShoot());
            }
        }
        // Jika jarak ke pemain lebih dari jarak tembak
        else
        {
            // Kejar pemain dan hentikan proses menembak dan peringatan
            enemy.stoppingDistance = 0f; // Tidak ada jarak berhenti saat mengejar
            enemy.SetDestination(player.position);
            StopShooting();              // Hentikan proses menembak
        }
    }

    // Coroutine untuk menampilkan logo danger, lalu menembak
    IEnumerator ShowWarningThenShoot()
    {
        isWarning = true;

        // Tampilkan logo danger
        dangerImage.SetActive(true);

        // Segera mulai menembak tanpa jeda
        isWarning = false;

        if (!isShooting)
        {
            StartCoroutine(Shoot());
        }

        // Karena ini adalah IEnumerator, tambahkan return null di akhir
        yield return null;
    }

    // Coroutine untuk menembak secara berkala
    IEnumerator Shoot()
    {
        isShooting = true;
        while (isShooting)
        {
            // Membuat peluru dan mengatur kecepatan peluru
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }

            // Tunggu sesuai kecepatan tembak
            yield return new WaitForSeconds(fireRate);
        }
    }

    // Fungsi untuk menghentikan proses menembak dan peringatan
    void StopShooting()
    {
        if (isShooting || isWarning)
        {
            StopAllCoroutines();         // Hentikan semua coroutine
            dangerImage.SetActive(false); // Pastikan logo danger mati
            isShooting = false;           // Reset status menembak
            isWarning = false;            // Reset status peringatan
        }
    }
}
