using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab peluru
    public float bulletSpeed = 5f;
    public float bulletLife = 2f;
    public int numberOfBullets = 6; // Jumlah peluru yang ditembakkan
    public float spreadAngle = 60f; // Sudut penyebaran peluru

    void Start()
    {
        SpawnBullets();
    }

    void SpawnBullets()
    {
        // Loop untuk menembakkan beberapa peluru
        for (int i = 0; i < numberOfBullets; i++)
        {
            // Hitung sudut untuk setiap peluru
            float angle = i * (spreadAngle / (numberOfBullets - 1)) - (spreadAngle / 2);

            // Buat arah peluru berdasarkan sudut
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            // Instansiasi peluru
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BulletEnemy2 bulletScript = bullet.GetComponent<BulletEnemy2>();
            bulletScript.direction = direction;
            bulletScript.speed = bulletSpeed;
            bulletScript.bulletLife = bulletLife;
        }
    }
}
