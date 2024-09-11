using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector2 InputVector { get; private set; }
    public Vector3 MousePosition { get; private set; }
    public bool IsRunning { get; private set; }

    // Update is called once per frame
    void Update()
    {
        // Ambil input horizontal dan vertikal
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        InputVector = new Vector2(h, v);

        // Ambil posisi mouse
        MousePosition = Input.mousePosition;

        // Cek apakah tombol Shift ditekan
        IsRunning = Input.GetKey(KeyCode.LeftShift);
    }
}