using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy2 : MonoBehaviour
{
    public float bulletLife = 1f;
    public Vector3 direction; // Arah perjalanan peluru
    public float speed = 1f;
    public float curveAmplitude = 1f; // Amplitudo untuk gerakan kurva
    public float curveFrequency = 1f; // Frekuensi untuk gerakan kurva

    private Vector3 spawnPoint;
    private float timer = 0f;

    void Start()
    {
        spawnPoint = transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Update()
    {
        if (timer > bulletLife) Destroy(this.gameObject);
        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    // Fungsi untuk menggerakkan peluru dengan pola kurva
    private Vector3 Movement(float timer)
    {
        // Gerakan lurus ditambah gerakan sinusoidal di sumbu Y
        Vector3 curvedMovement = spawnPoint + (timer * speed * direction);
        curvedMovement.y += Mathf.Sin(timer * curveFrequency) * curveAmplitude; // Gerakan kurva pada sumbu Y

        return curvedMovement;
    }
}