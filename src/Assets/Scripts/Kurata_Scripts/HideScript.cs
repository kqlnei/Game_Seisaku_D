using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class HideScript : MonoBehaviour
{
    private bool isPlayerInside = false;
    private Transform InSidePoint;
    private Transform OutSidePoint;
    private Transform nowPoint;
    public bool HideMode = false;

    [SerializeField] float moveSpeed = 5.0f;

    private Playercontroller playerController;
    private BoxCollider boxCollider;
    
    public Image hideImage;

    private CapsuleCollider capsuleCollider;

    private bool isMoving = false; // �ړ����̃t���O

    public AudioClip Armorse;
    public AudioClip Armorvc;
    private AudioSource audioSource;
    public AudioSource audioSource1;

    Animator ArmorAnimator;
    private Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        hideImage.gameObject.SetActive(false);
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerController = GetComponent<Playercontroller>();
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInside && !isMoving)
        {
            // �v���C���[���G���A���ɂ���ꍇ�̏���
            if (Gamepad.current.buttonSouth.wasReleasedThisFrame)// || Input.GetKeyDown(KeyCode.Z)
            {
                HideMode = !HideMode;


                ChangeHide();
            }

        }
    }

    void ChangeHide()
    {
        if (HideMode)
        {
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            if (boxCollider != null)
            {
                boxCollider.isTrigger = true;
            }
            m_Animator.SetBool("Hide", true);
            capsuleCollider.enabled = false;
            StartCoroutine(MoveToTarget(nowPoint.position, InSidePoint.position));
            audioSource.Play();
            audioSource1.PlayOneShot(Armorvc);

        }
        else
        {
            if (boxCollider != null)
            {
                boxCollider.isTrigger = false;
            }
            capsuleCollider.enabled = true;
            m_Animator.SetBool("Hide", true);
            StartCoroutine(MoveToTarget(nowPoint.position, OutSidePoint.position));
            this.transform.GetChild(2).gameObject.SetActive(true);
            if (ArmorAnimator != null)
            {
                ArmorAnimator.SetBool("possession", false);
            }
            audioSource.Stop();
            audioSource1.PlayOneShot(Armorvc);
        }
    }

    // MoveToTarget ���\�b�h
    IEnumerator MoveToTarget(Vector3 nowPosition, Vector3 targetPosition)
    {
        targetPosition.y = 1f; // y���W���Œ肷��
        isMoving = true; // �ړ����̃t���O���Z�b�g
        transform.LookAt(targetPosition);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
           
            transform.position = Vector3.Slerp(nowPosition, targetPosition, moveSpeed * Time.deltaTime);
            nowPosition = transform.position;

            yield return null; // 1�t���[���ҋ@
        }

        transform.position = targetPosition; // �ŏI�I�Ȉʒu��ڕW�̈ʒu�ɐݒ�
        isMoving = false; // �ړ����I�������t���O�����Z�b�g
        m_Animator.SetBool("Hide", false);


        if (HideMode)
        {
            this.transform.GetChild(2).gameObject.SetActive(false);

            if (ArmorAnimator != null)
            {
                ArmorAnimator.SetBool("possession", true);
            }
        }

        // �ړ�������������v���C���[�R���g���[���[��L���ɂ���
        if (!HideMode)
        {
            targetPosition.y = 1f; // y���W���Œ肷��
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
                    
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Hide")
        {
            Transform parentTransform = col.transform.parent;
            boxCollider = parentTransform.GetComponent<BoxCollider>();

            Transform ArmorTransform = parentTransform.GetChild(0);
            ArmorAnimator = ArmorTransform.GetComponent<Animator>();

            isPlayerInside = true;
            hideImage.gameObject.SetActive(true);

            OutSidePoint = col.transform.GetChild(0);
            InSidePoint = col.transform.GetChild(1);
            nowPoint = this.transform;


            audioSource = col.gameObject.GetComponent<AudioSource>();
            audioSource.clip = Armorse;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        hideImage.gameObject.SetActive(false);
    }
}