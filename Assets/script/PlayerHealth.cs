using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Fungsi untuk mengurangi health dan handle kematian player
    IEnumerator UpdateHealthUI()
    {
        float targetValue = currentHealth;
        float epsilon = 0.1f; // Nilai selisih kecil

        while (Mathf.Abs(healthSlider.value - targetValue) > epsilon)
        {
            healthSlider.value = Mathf.MoveTowards(healthSlider.value, targetValue, 5f * Time.deltaTime);
            yield return null;
        }

        healthSlider.value = targetValue; // Pastikan nilai slider mencapai targetValue
    }

    // Fungsi untuk mengurangi health dan handle kematian player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        StopCoroutine("UpdateHealthUI"); // Hentikan coroutine sebelumnya jika ada
        StartCoroutine(UpdateHealthUI());

        if (currentHealth <= 0)
        {
            Die(); // Panggil fungsi Die() jika player sudah mati
        }

    }

    // Fungsi untuk menangani kematian player (misalnya:ilangkan player)
    void Die()
    {
        healthSlider.value = 0;
        // Contoh: menonaktifkan GameObject player
        gameObject.SetActive(false);

        // Atau bisa juga destroy GameObject player:
     }
}