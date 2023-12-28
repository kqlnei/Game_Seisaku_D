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

    // glass_break�����Z�b�g���邽�߂̃^�C�}�[
    private float glassBreakTimer = 0f;
    private float glassBreakDuration = 2f; // �K�v�ɉ����Ď��Ԃ𒲐����Ă�������

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
            // �v���C���[���G���A���ɂ���ꍇ�̏���
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

        // glass_break��true�̏ꍇ�A��莞�Ԍ�Ƀ��Z�b�g
        if (glass_break)
        {
            glassBreakTimer += Time.deltaTime;

            if (glassBreakTimer >= glassBreakDuration)
            {
                glass_break = false;
                glassBreakTimer = 0f; // �^�C�}�[�����Z�b�g
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
            
            // �ڐG�����I�u�W�F�N�g�̐e�I�u�W�F�N�g���擾
            parentTransform = col.transform.parent;

            // �e�I�u�W�F�N�g�ɃA�^�b�`����Ă���Animator�R���|�[�l���g���擾
            parentAnimator = parentTransform.GetComponent<Animator>();

            Transform FallCpllider = parentTransform.GetChild(4);
            Debug.Log(FallCpllider);
            sphereCollider = FallCpllider.GetComponent<SphereCollider>();
            
            // 1���̎q�I�u�W�F�N�g��Transform���擾
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