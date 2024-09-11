using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Kesehatan maksimum musuh
    private float currentHealth;   // Kesehatan saat ini

    void Start()
    {
        // Inisialisasi kesehatan saat ini dengan kesehatan maksimum
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        // Kurangi kesehatan saat terkena damage
        currentHealth -= damage;

        // Cek apakah kesehatan musuh sudah habis
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Logika ketika musuh mati
        // Misalnya: menghancurkan objek musuh
        Destroy(gameObject);
    }
}