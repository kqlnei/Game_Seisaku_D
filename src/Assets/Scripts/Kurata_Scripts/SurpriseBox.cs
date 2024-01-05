using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SurpriseBox : MonoBehaviour
{
    public GameObject Surprise_Box;
    private bool canSpawn = true;
    private float cooldownTimer = 5f;

    public Image UIobj;
    public bool roop;

    public AudioClip Actionvc;
    public AudioSource audioSource1;

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && Input.GetKeyDown(KeyCode.Z))//(canSpawn && Gamepad.current.buttonWest.wasReleasedThisFrame)
        {
            audioSource1.PlayOneShot(Actionvc);
            // �v���n�u�𐶐�
            Instantiate(Surprise_Box, transform.position, Quaternion.identity);
            canSpawn = false;
            StartCoroutine(StartCooldown());
        }
        if(!canSpawn)
        {
            // UI�̕\����Ԃ𐧌�
            UIobj.enabled = true;  // �f�t�H���g�͕\��

            UIobj.fillAmount -= 1.0f / cooldownTimer * Time.deltaTime;

            // UIobj.fillAmount��0���傫���ꍇ��Image��\���itrue�j�A�����łȂ��ꍇ�͔�\���ifalse�j
            UIobj.enabled = UIobj.fillAmount > 0;
        }
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldownTimer);
        UIobj.fillAmount = 1;
        canSpawn = true;

        // �J�E���g�_�E�����I�������̂�Image���\���ɂ���
        UIobj.enabled = false;
    }
}
