using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;
    private Animator _animator;
    private AudioSource _audioSource; // Referensi ke komponen AudioSource

    [SerializeField]
    private bool RotateTowardMouse;

    [SerializeField]
    private float WalkSpeed = 2f;
    [SerializeField]
    private float RunSpeed = 5f;
    [SerializeField]
    private float RotationSpeed = 700f;

    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private AudioClip FootstepSound; // Klip suara langkah kaki

    private float currentSpeed;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _animator = GetComponent<Animator>(); // Referensi komponen Animator
        _audioSource = GetComponent<AudioSource>(); // Referensi komponen AudioSource
    }

    // Update dipanggil sekali per frame
    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);

        UpdateAnimation(movementVector.magnitude); // Update animasi berdasarkan gerakan
        HandleFootstepAudio(movementVector.magnitude); // Atur audio langkah kaki

        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);
        }
        if (RotateTowardMouse)
        {
            RotateFromMouseVector();
        }
    }

    private void RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        // Periksa apakah pemain menekan tombol Shift untuk berlari
        currentSpeed = _input.IsRunning ? RunSpeed : WalkSpeed;

        var speed = currentSpeed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
    }

    private void UpdateAnimation(float movementMagnitude)
    {
        if (_animator != null)
        {
            bool isWalking = movementMagnitude > 0 && currentSpeed == WalkSpeed;
            bool isRunning = movementMagnitude > 0 && currentSpeed == RunSpeed;

            _animator.SetBool("IsWalking", isWalking);
            _animator.SetBool("IsRunning", isRunning);
        }
    }

    private void HandleFootstepAudio(float movementMagnitude)
    {
        if (movementMagnitude > 0 && !_audioSource.isPlaying)
        {
            _audioSource.clip = FootstepSound;
            _audioSource.Play();
        }
        else if (movementMagnitude == 0 && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
