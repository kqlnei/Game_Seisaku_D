using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower_Audio : MonoBehaviour
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
            PoltergeisScript poltergeisScript = other.GetComponentInParent<PoltergeisScript>();

            // poltergeisScript��null�łȂ����m�F
            if (poltergeisScript != null)
            {
                // PoltergeisScript��glass_break�ϐ��ɃA�N�Z�X
                bool isGlassBroken = poltergeisScript.glass_break;

                // glass_break��True�ɂȂ�A���܂������Đ�����Ă��Ȃ��ꍇ
                if (isGlassBroken && !hasPlayedSound)
                {
                    // �R���[�`�����J�n���ĉ����Đ�
                    StartCoroutine(PlayAudioWithDelay());

                    BoxCollider boxCollider = GetComponent<BoxCollider>();
                    if (boxCollider != null)
                    {
                        boxCollider.enabled = false;
                    }
                }
            }
        }
    }

    IEnumerator PlayAudioWithDelay()
    {
        // ���b�҂��Ă��特���Đ�
        yield return new WaitForSeconds(0.5f); // 3�b�҂�

        // AudioSource���擾���ĉ����Đ�
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
            hasPlayedSound = true; // �����Đ����ꂽ�t���O���Z�b�g
        }
    }
}
