using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Audio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // ��x���������Đ����ꂽ���ǂ������Ǘ�����t���O
    private bool hasPlayedSound = false;

    void OnTriggerStay(Collider other)
    {
        // �v���C���[�^�O�̃I�u�W�F�N�g�ɐG��Ă��邩�ǂ����m�F
        if (other.CompareTag("Player"))
        {
            // PoltergeisScript�̃C���X�^���X���擾
            KeyAndDoorScript keyanddoorScript = other.GetComponentInParent<KeyAndDoorScript>();

            // poltergeisScript��null�łȂ����m�F
            if (keyanddoorScript != null)
            {
                // PoltergeisScript��glass_break�ϐ��ɃA�N�Z�X
                bool isGlassBroken = keyanddoorScript.Door_Open;

                // glass_break��True�ɂȂ�A���܂������Đ�����Ă��Ȃ��ꍇ
                if (isGlassBroken && !hasPlayedSound)
                {
                    // �R���[�`�����J�n���ĉ����Đ�
                    StartCoroutine(PlayAudioWithDelay());

                }
            }
        }
    }

    IEnumerator PlayAudioWithDelay()
    {
        // ���b�҂��Ă��特���Đ�
        yield return new WaitForSeconds(0f); // 3�b�҂�

        // AudioSource���擾���ĉ����Đ�
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
            hasPlayedSound = true; // �����Đ����ꂽ�t���O���Z�b�g
        }
    }
}
