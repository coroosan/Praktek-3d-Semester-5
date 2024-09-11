using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float bulletSpeed = 20f; // Kecepatan peluru
    public float maxDistance = 50f; // Jarak maksimum peluru aktif
    public int damage = 10;         // Kerusakan yang diberikan peluru

    private Rigidbody rb;
    private float distanceTraveled = 0f; // Jarak yang telah ditempuh peluru
    private Vector3 bulletDirection;     // Arah peluru awal

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Menonaktifkan gravitasi
        }

        // Temukan pemain sebagai target peluru
        Transform target = GameObject.FindWithTag("Player")?.transform;

        if (target != null)
        {
            // Hitung arah ke pemain saat ini dan setel kecepatan peluru (hanya dilakukan sekali)
            bulletDirection = (target.position - transform.position).normalized;
            rb.velocity = bulletDirection * bulletSpeed;
        }
    }

    void FixedUpdate()
    {
        // Hitung jarak yang telah ditempuh peluru
        distanceTraveled += rb.velocity.magnitude * Time.deltaTime;

        // Hapus peluru jika jarak maksimum tercapai
        if (distanceTraveled > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Jika peluru mengenai pemain, panggil metode untuk mengurangi kesehatan
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Hapus peluru setelah terkena
            Destroy(gameObject);
        }
    }
}
