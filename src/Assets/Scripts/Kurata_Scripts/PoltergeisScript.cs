using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PoltergeisScript : MonoBehaviour
{
    private bool isPlayerInside = false;

    private Animator parentAnimator;

    private SphereCollider sphereCollider;

    public bool glass_break = false;

    // glass_breakをリセットするためのタイマー
    private float glassBreakTimer = 0f;
    private float glassBreakDuration = 2f; // 必要に応じて時間を調整してください

    public Image polImage;
    private Color imageColor;

    public AudioClip Poltervc;
    public AudioClip Actionvc;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    private Animator m_Animator;

    private Transform parentTransform;
    private Transform childTransform;
    // Start is called before the first frame update
    void Start()
    {
        polImage.gameObject.SetActive(true);
        m_Animator = GetComponent<Animator>();

        imageColor = polImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInside)
        {
            // プレイヤーがエリア内にいる場合の処理
            if (Gamepad.current.buttonSouth.wasReleasedThisFrame)// || Input.GetKeyDown(KeyCode.Z)
            {

                if (parentAnimator != null)
                {
                    parentAnimator.SetBool("isFall", true);

                    //polImage.gameObject.SetActive(false);
                    // 透明度を40に変更
                    imageColor.a = 40f / 255f;
                    polImage.color = imageColor;


                    if (sphereCollider != null)
                    {
                        sphereCollider.enabled = true;
                    }
                }

                if (childTransform != null)
                {
                    Vector3 targetPosition = new Vector3(childTransform.position.x, transform.position.y, childTransform.position.z);
                    transform.LookAt(targetPosition);
                }

                glass_break = true;
                audioSource1.PlayOneShot(Poltervc);
                isPlayerInside = false;
                m_Animator.SetBool("Pol", true);
            }

        }

        // glass_breakがtrueの場合、一定時間後にリセット
        if (glass_break)
        {
            glassBreakTimer += Time.deltaTime;

            if (glassBreakTimer >= glassBreakDuration)
            {
                glass_break = false;
                glassBreakTimer = 0f; // タイマーをリセット
                m_Animator.SetBool("Pol", false);
            }

        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Pol")
        {
            isPlayerInside = true;

            // 透明度を255に変更
            imageColor.a = 255f / 255f;
            polImage.color = imageColor;
            audioSource2.PlayOneShot(Actionvc);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Pol")
        {

            // 接触したオブジェクトの親オブジェクトを取得
            parentTransform = col.transform.parent;

            // 親オブジェクトにアタッチされているAnimatorコンポーネントを取得
            parentAnimator = parentTransform.GetComponent<Animator>();

            Transform FallCpllider = parentTransform.GetChild(4);
            Debug.Log(FallCpllider);
            sphereCollider = FallCpllider.GetComponent<SphereCollider>();

            // 1個下の子オブジェクトのTransformを取得
            childTransform = parentTransform.transform.GetChild(6);
            Debug.Log(childTransform);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        //polImage.gameObject.SetActive(false);

        // 透明度を40に変更
        imageColor.a = 40f / 255f;
        polImage.color = imageColor;


    }
}