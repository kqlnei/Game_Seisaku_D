using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Goal : MonoBehaviour
{
    public Image goalImage;
    // Collider��Goal�G���A�ɓ�������true�ɂ���t���O
    private bool goalEntered = false;
    public bool goal = true;

    public AudioClip goal_se;
    private AudioSource audioSource;

    private float triggerDelay = 2f; // �g���K�[��ݒ肷��܂ł̑ҋ@����

    public CinemachineVirtualCameraBase vcam3;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // OnTriggerEnter��Collider������Collider�ɓ������u�ԂɌĂ΂��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �������͑��̃^�O���g�p
        {
            // �v���C���[��Goal�G���A�ɓ�������t���O��true�ɐݒ�
            goalEntered = true;
            // �����Ō��ʃV�[���ɑJ�ڂ��郁�\�b�h���Ăяo��
            ShowGoalText();

            DisablePlayerController(other);  // �v���C���[�R���g���[���[�𖳌������郁�\�b�h���Ăяo��
            StartCoroutine(MovePlayerToTarget(other));// �v���C���[��ڕW�n�_�܂ňړ������郁�\�b�h���Ăяo��

            StartCoroutine(SetAnimatorTriggerWithDelay(other, triggerDelay));


            Invoke("Result", 7f);
            goal = false;
        }
    }

    private void Result()
    {
        // �t���O��true�̏ꍇ�A���ʃV�[���ɑJ��
        if (goalEntered)
        {
            SceneManager.LoadSceneAsync("ResultScene");
        }
    }

    private void ShowGoalText()
    {
        // goalText�����݂��A�܂��\������Ă��Ȃ��ꍇ
        if (goalImage != null && !goalImage.gameObject.activeSelf)
        {
            goalImage.gameObject.SetActive(true); // �e�L�X�g��\������

            audioSource.PlayOneShot(goal_se);
        }
    }

    private IEnumerator MovePlayerToTarget(Collider other)
    {
        Transform targetTransform = transform.GetChild(0); // 0�͎q�I�u�W�F�N�g�̃C���f�b�N�X�B�K�v�ɉ����ĕύX
        if (targetTransform != null)
        {
            Vector3 targetPosition = targetTransform.position;

            float duration = 3f; // �ړ��ɂ����鎞��
            float elapsedTime = 0f;

            Vector3 initialPosition = other.transform.position;

            vcam3.Priority = 5;

            while (elapsedTime < duration)
            {
                other.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
                other.transform.LookAt(targetPosition); // �ړI�n�̕���������

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // �ŏI�n�_��180�x��]
            other.transform.Rotate(0f, 180f, 0f);

           
        }
    }

    private void DisablePlayerController(Collider other)
    {
        // �v���C���[�R���g���[���[�X�N���v�g�𖳌�������
        Playercontroller playerController = other.GetComponent<Playercontroller>();

        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    private IEnumerator SetAnimatorTriggerWithDelay(Collider other, float delay)
    { 
        yield return new WaitForSeconds(delay);

        // Animator��ClearTrigger��True�ɐݒ肷��
        Animator animator = other.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Clear", true);
        }
    }
}
