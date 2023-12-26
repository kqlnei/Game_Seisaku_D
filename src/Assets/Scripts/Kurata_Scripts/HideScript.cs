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

    private bool isMoving = false; // 移動中のフラグ

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
            // プレイヤーがエリア内にいる場合の処理
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

    // MoveToTarget メソッド
    IEnumerator MoveToTarget(Vector3 nowPosition, Vector3 targetPosition)
    {
        targetPosition.y = 1f; // y座標を固定する
        isMoving = true; // 移動中のフラグをセット
        transform.LookAt(targetPosition);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
           
            transform.position = Vector3.Slerp(nowPosition, targetPosition, moveSpeed * Time.deltaTime);
            nowPosition = transform.position;

            yield return null; // 1フレーム待機
        }

        transform.position = targetPosition; // 最終的な位置を目標の位置に設定
        isMoving = false; // 移動が終わったらフラグをリセット
        m_Animator.SetBool("Hide", false);


        if (HideMode)
        {
            this.transform.GetChild(2).gameObject.SetActive(false);

            if (ArmorAnimator != null)
            {
                ArmorAnimator.SetBool("possession", true);
            }
        }

        // 移動が完了したらプレイヤーコントローラーを有効にする
        if (!HideMode)
        {
            targetPosition.y = 1f; // y座標を固定する
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