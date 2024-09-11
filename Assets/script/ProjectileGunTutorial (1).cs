using UnityEngine;
using TMPro;

public class ProjectileGunTutorial : MonoBehaviour
{
    public GameObject peluru;
    // Kekuatan tembak
    public float kekuatanTembak, penyebaran;

    // Statistik senjata
    public float waktuAntarTembakan, waktuIsiUlang, waktuAntarPeluru;
    public int ukuranMagazine, peluruPerTap;
    public bool bolehTahanTombol;

    int peluruTersisa, peluruDitembakkan;

    // Bools
    bool menembak, siapMenembak, mengisiUlang;

    // Referensi
    public Camera kameraIsometrik;
    public Transform titikSerang;

    // Grafik
    public TextMeshProUGUI tampilanAmunisi;

    // Bug fixing :D
    public bool allowInvoke = true;

    private void Awake()
    {
        // Pastikan magazine penuh
        peluruTersisa = ukuranMagazine;
        siapMenembak = true;
    }

    private void Update()
    {
        InputSaya();

        // Set tampilan amunisi jika ada
        if (tampilanAmunisi != null)
            tampilanAmunisi.SetText(peluruTersisa / peluruPerTap + " / " + ukuranMagazine / peluruPerTap);
    }

    private void InputSaya()
    {
        // Cek apakah diperbolehkan menahan tombol
        if (bolehTahanTombol)
            menembak = Input.GetKey(KeyCode.Mouse0);
        else
            menembak = Input.GetKeyDown(KeyCode.Mouse0);

        // Isi ulang
        if (Input.GetKeyDown(KeyCode.R) && peluruTersisa < ukuranMagazine && !mengisiUlang)
            IsiUlang();

        // Isi ulang otomatis jika menembak tanpa amunisi
        if (siapMenembak && menembak && !mengisiUlang && peluruTersisa <= 0)
            IsiUlang();

        // Menembak
        if (siapMenembak && menembak && !mengisiUlang && peluruTersisa > 0)
        {
            peluruDitembakkan = 0;
            Menembak();
        }
    }

    private void Menembak()
    {
        siapMenembak = false;

        // Raycast dari posisi mouse
        Ray ray = kameraIsometrik.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 titikTarget;
        if (Physics.Raycast(ray, out hit))
            titikTarget = hit.point;
        else
            titikTarget = ray.GetPoint(75); // Jika tidak ada yang terkena, ambil titik jauh

        // Hitung arah tembakan dari titik serang ke titik target
        Vector3 arahTembak = titikTarget - titikSerang.position;

        // Tambahkan penyebaran di sumbu X dan Z (horizontal)
        float x = Random.Range(-penyebaran, penyebaran);
        float z = Random.Range(-penyebaran, penyebaran);

        Vector3 arahDenganPenyebaran = arahTembak + new Vector3(x, 0, z); // Tidak ada penyebaran di sumbu Y (vertikal)

        // Buat peluru
        GameObject peluruSaatIni = Instantiate(peluru, titikSerang.position, Quaternion.identity);
        peluruSaatIni.transform.forward = arahDenganPenyebaran.normalized;

        // Tambahkan gaya ke peluru
        peluruSaatIni.GetComponent<Rigidbody>().AddForce(arahDenganPenyebaran.normalized * kekuatanTembak, ForceMode.Impulse);

        peluruTersisa--;
        peluruDitembakkan++;

        // Invoke reset tembakan jika tidak sedang di-invoke
        if (allowInvoke)
        {
            Invoke("ResetTembakan", waktuAntarTembakan);
            allowInvoke = false;
        }

        // Jika lebih dari satu peluru per tap, ulangi fungsi menembak
        if (peluruDitembakkan < peluruPerTap && peluruTersisa > 0)
            Invoke("Menembak", waktuAntarPeluru);
    }

    private void ResetTembakan()
    {
        siapMenembak = true;
        allowInvoke = true;
    }

    private void IsiUlang()
    {
        mengisiUlang = true;
        Invoke("SelesaiIsiUlang", waktuIsiUlang);
    }

    private void SelesaiIsiUlang()
    {
        peluruTersisa = ukuranMagazine;
        mengisiUlang = false;
    }
}