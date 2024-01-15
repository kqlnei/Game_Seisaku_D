using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    // プレイヤーの移動関連のパラメータ
    public float turnSpeed = 20f;
    public float moveSpeed = 5f;
    public float stoppingTime = 0.1f;

    // フェード関連のパラメータ
    public float fadeOutTime = 1.0f;

    // 音声関連の変数
    public AudioClip walk;
    private AudioSource audioSource;

    // プレイヤーの状態を示す変数
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private Vector3 movementInput;
    private Quaternion rotationInput = Quaternion.identity;
    private Vector3 velocity;

    void Start()
    {
        InitializeComponents();
        InitializeAudioSource();

        // SecretCommandScriptが有効なら速度を調整
        if (SecretComandScript.Comand)
        {
            turnSpeed = 30f;
            moveSpeed = 7.5f;
        }
    }

    void FixedUpdate()
    {
        HandleInput();

        UpdateMovementAnimation();

        RotatePlayer();

        MovePlayer();

        ManageAudio();

        OnAnimatorMove();
    }

    void InitializeComponents()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void InitializeAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = walk;
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = CorrectInputDirection(new Vector3(horizontal, 0f, vertical));
        inputDirection.Normalize();

        movementInput.Set(inputDirection.x, 0f, inputDirection.z);
    }

    void UpdateMovementAnimation()
    {
        bool isWalking = movementInput.magnitude > 0;
        playerAnimator.SetBool("Walking", isWalking);
    }

    void RotatePlayer()
    {
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movementInput, turnSpeed * Time.deltaTime, 0f);
        rotationInput = Quaternion.LookRotation(desiredForward);
    }

    void MovePlayer()
    {
        Vector3 movement = movementInput * moveSpeed * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + movement);

        if (!movementInput.Equals(Vector3.zero))
        {
            velocity = movementInput * moveSpeed;
        }
        else
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, stoppingTime * Time.fixedDeltaTime);
            playerRigidbody.MovePosition(playerRigidbody.position + velocity * Time.fixedDeltaTime);
        }
    }

    void ManageAudio()
    {
        if (movementInput.magnitude > 0 && !audioSource.isPlaying)
        {
            audioSource.Play();
            StartCoroutine(FadeIn(audioSource, fadeOutTime));
        }
        else if (movementInput.magnitude == 0 && audioSource.isPlaying)
        {
            StartCoroutine(FadeOut(audioSource, fadeOutTime));
        }
    }

    void OnAnimatorMove()
    {
        // アニメーターが移動したら、プレイヤーも回転を適用する
        playerRigidbody.MoveRotation(rotationInput);
    }

    Vector3 CorrectInputDirection(Vector3 inputDirection)
    {
        float cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;
        Quaternion correction = Quaternion.Euler(0f, cameraRotationY, 0f);
        Vector3 correctedDirection = correction * inputDirection;

        return correctedDirection.normalized;
    }

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

    IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0.1f;

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
