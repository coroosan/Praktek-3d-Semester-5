using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    public float explosionRange;
    public float explosionForce;
    public float maxLifetime = 5f;
    private float bulletSpeed = 20f;

    private void Start()
    {
        // Set kecepatan awal peluru
        rb.velocity = transform.forward * bulletSpeed;

        // Menghancurkan peluru setelah batas waktu
        Invoke("Explode", maxLifetime);
    }

    private void Explode()
    {
        // Memunculkan efek ledakan
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        // Menambahkan gaya ledakan pada musuh di sekitar
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        foreach (Collider enemy in enemies)
        {
            if (enemy.GetComponent<Rigidbody>())
                enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        // Menghancurkan peluru setelah ledakan
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Abaikan tabrakan dengan objek tertentu seperti Player
        if (collision.collider.CompareTag("Player")) return;

        // Jika bertabrakan dengan musuh, meledak
        if (collision.collider.CompareTag("Enemy")) Explode();
    }

    private void OnDrawGizmosSelected()
    {
        // Menggambar radius ledakan di Scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
