using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Goal : MonoBehaviour
{
    public TMP_Text goalText;
    // Collider��Goal�G���A�ɓ�������true�ɂ���t���O
    private bool goalEntered = false;
    public bool goal = true;

    public AudioClip goal_se;
    private AudioSource audioSource;

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
            Invoke("Result", 5f);
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
        if (goalText != null && !goalText.gameObject.activeSelf)
        {
            goalText.gameObject.SetActive(true); // �e�L�X�g��\������

            audioSource.PlayOneShot(goal_se);
        }
    }
}
