using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float moveSpeed = 5f;
    public float stoppingTime = 0.1f; // Adjust this value for the desired stopping time

    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Movement;
    private Quaternion m_Rotation = Quaternion.identity;
    private Vector3 m_Velocity;


    public AudioClip walk;
    private AudioSource audioSource;

    private bool isWalking;

    // 音量の変化にかかる時間
    public float fadeOutTime = 1.0f;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = walk;
        if (SecretComandScript.Comand)
        {
            turnSpeed = 30f;
            moveSpeed = 7.5f;
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = CorrectInputDirection(new Vector3(horizontal, 0f, vertical));
        inputDirection.Normalize();

        m_Movement.Set(inputDirection.x, 0f, inputDirection.z);

        bool hasHorizontalInput = !Mathf.Approximately(inputDirection.x, 0f);
        bool hasVerticalInput = !Mathf.Approximately(inputDirection.z, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("Walking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        Move();

        // 音の再生とフェードアウト
        if (isWalking && !audioSource.isPlaying)
        {
            audioSource.Play();
            StartCoroutine(FadeIn(audioSource, fadeOutTime));
        }
        else if (!isWalking && audioSource.isPlaying)
        {
            StartCoroutine(FadeOut(audioSource, fadeOutTime));
        }
    }

    void Move()
    {
        Vector3 movement = m_Movement * moveSpeed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

        // Apply inertia to slow down when there is no input
        if (!m_Movement.Equals(Vector3.zero))
        {
            m_Velocity = m_Movement * moveSpeed;
        }
        else
        {
            m_Velocity = Vector3.Lerp(m_Velocity, Vector3.zero, stoppingTime * Time.fixedDeltaTime);
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Velocity * Time.fixedDeltaTime);
        }
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    Vector3 CorrectInputDirection(Vector3 inputDirection)
    {
        float cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;
        Quaternion correction = Quaternion.Euler(0f, cameraRotationY, 0f);
        Vector3 correctedDirection = correction * inputDirection;

        return correctedDirection.normalized;
    }

     // 音のフェードアウト
    IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    // 音のフェードイン
    IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0.1f; // フェードイン時の開始音量（お好みで調整）

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
