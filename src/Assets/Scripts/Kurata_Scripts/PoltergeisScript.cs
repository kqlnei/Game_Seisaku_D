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
    // Start is called before the first frame update
    void Start()
    {
        polImage.gameObject.SetActive(false);
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
                glass_break = true;
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
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Pol")
        {
            polImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Pol")
        {
            isPlayerInside = true;
           

            // �ڐG�����I�u�W�F�N�g�̐e�I�u�W�F�N�g���擾
            Transform parentTransform = col.transform.parent;

            // �e�I�u�W�F�N�g�ɃA�^�b�`����Ă���Animator�R���|�[�l���g���擾
            parentAnimator = parentTransform.GetComponent<Animator>();

            Transform FallCpllider = parentTransform.GetChild(4);
            Debug.Log(FallCpllider);
            sphereCollider = FallCpllider.GetComponent<SphereCollider>();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        polImage.gameObject.SetActive(false);

    }
}