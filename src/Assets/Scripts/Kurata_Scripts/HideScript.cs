using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HideScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private Image hideImage;
    [SerializeField] private AudioClip armorseClip;
    [SerializeField] private AudioClip armorvcClip;
    [SerializeField] private AudioClip actionvcClip;

    private bool isPlayerInside = false;
    private bool isMoving = false;
    public bool HideMode = false;

    private Transform insidePoint;
    private Transform outsidePoint;
    private Transform currentPoint;

    private Playercontroller playerController;
    private BoxCollider boxCollider;
    private CapsuleCollider capsuleCollider;

    private AudioSource audioSource;
    private AudioSource audioSource1;
    private Animator armorAnimator;
    private Animator animator;

    private Color imageColor;

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        hideImage.gameObject.SetActive(true);
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerController = GetComponent<Playercontroller>();
        animator = GetComponent<Animator>();
        imageColor = hideImage.color;
    }

    private void Update()
    {
        if (isPlayerInside && !isMoving)
        {
            HandlePlayerInside();
        }
    }

    private void HandlePlayerInside()
    {
        if (Gamepad.current.buttonSouth.wasReleasedThisFrame)
        {
            HideMode = !HideMode;
            ChangeHide();
        }
    }

    private void ChangeHide()
    {
        if (HideMode)
        {
            SetHideMode();
        }
        else
        {
            SetUnhideMode();
        }
    }

    private void SetHideMode()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }

        animator.SetBool("Hide", true);
        capsuleCollider.enabled = false;
        StartCoroutine(MoveToTarget(currentPoint.position, insidePoint.position));

        audioSource.Play();
        audioSource1.PlayOneShot(armorvcClip);
    }

    private void SetUnhideMode()
    {
        if (boxCollider != null)
        {
            boxCollider.isTrigger = false;
        }

        capsuleCollider.enabled = true;
        animator.SetBool("Hide", true);
        StartCoroutine(MoveToTarget(currentPoint.position, outsidePoint.position));

        this.transform.GetChild(2).gameObject.SetActive(true);

        if (armorAnimator != null)
        {
            armorAnimator.SetBool("possession", false);
        }

        audioSource.Stop();
        audioSource1.PlayOneShot(armorvcClip);
    }

    IEnumerator MoveToTarget(Vector3 nowPosition, Vector3 targetPosition)
    {
        targetPosition.y = 1f;
        isMoving = true;
        transform.LookAt(targetPosition);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.Slerp(nowPosition, targetPosition, moveSpeed * Time.deltaTime);
            nowPosition = transform.position;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        animator.SetBool("Hide", false);

        if (HideMode)
        {
            this.transform.GetChild(2).gameObject.SetActive(false);

            if (armorAnimator != null)
            {
                armorAnimator.SetBool("possession", true);
            }
        }

        if (!HideMode)
        {
            targetPosition.y = 1f;
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hide")
        {
            audioSource.PlayOneShot(actionvcClip);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Hide")
        {
            SetInsidePointValues(col);
        }
    }

    private void SetInsidePointValues(Collider col)
    {
        Transform parentTransform = col.transform.parent;
        boxCollider = parentTransform.GetComponent<BoxCollider>();

        Transform armorTransform = parentTransform.GetChild(0);
        armorAnimator = armorTransform.GetComponent<Animator>();

        isPlayerInside = true;
        imageColor.a = 255f / 255f;
        hideImage.color = imageColor;

        outsidePoint = col.transform.GetChild(0);
        insidePoint = col.transform.GetChild(1);
        currentPoint = this.transform;

        audioSource = col.gameObject.GetComponent<AudioSource>();
        audioSource.clip = armorseClip;
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        imageColor.a = 40f / 255f;
        hideImage.color = imageColor;
    }
}
