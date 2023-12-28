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

    public AudioClip Poltervc;
    
    public AudioSource audioSource1;

    private Animator m_Animator;

    private Transform parentTransform;
    private Transform childTransform;
    // Start is called before the first frame update
    void Start()
    {
        polImage.gameObject.SetActive(false);
        m_Animator = GetComponent<Animator>();
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
                   
                    polImage.gameObject.SetActive(false);
                    if (sphereCollider != null)
                    {
                        sphereCollider.enabled = true;
                    }
                }

                if (childTransform != null)
                {
                    transform.LookAt(childTransform);
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
            polImage.gameObject.SetActive(true);
            isPlayerInside = true;
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
            childTransform = parentTransform.transform.GetChild(0);
            Debug.Log(childTransform);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        polImage.gameObject.SetActive(false);

    }
}