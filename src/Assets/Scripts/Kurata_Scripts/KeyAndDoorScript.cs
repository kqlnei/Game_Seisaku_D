using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyAndDoorScript : MonoBehaviour
{
    public GameObject[] zyomaeLocks;

    public GameObject Door;

    public int requiredKeys = 3;

    private int collectedKeys = 0;

    private bool keysCollected = false;

    public Image[] KeyImages;

    private Animator childAnimator;

    private BoxCollider doorCollider;

    public bool Door_Open = false;

    public AudioClip Keyse;
    public AudioClip Lockse;
    public AudioSource audioSource_Key;
    public AudioSource audioSource_Lock;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {  
            collectedKeys++;
            keysCollected = true;
            // �v���C���[�̎����Ă��錮���폜
            Destroy(other.gameObject);
            KeyImages[collectedKeys - 1].enabled = true;
            audioSource_Key.PlayOneShot(Keyse);
        }

        if (other.CompareTag("Door"))
        {
            // ���O���O��鏈�������s
            UnlockDoor();

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            // ���ׂĂ̌����擾�����ꍇ�A�����J���鏈��
            if (collectedKeys >= requiredKeys)
            {
                // �ڐG�����I�u�W�F�N�g�̎q�I�u�W�F�N�g���擾
                Transform childTransform = other.transform.GetChild(0);
                // �q�I�u�W�F�N�g�ɃA�^�b�`����Ă���Animator�R���|�[�l���g���擾
                childAnimator = childTransform.GetComponent<Animator>();

                if (childAnimator != null)
                {
                    childAnimator.SetBool("DoorOpen", true);
                }

                Door_Open = true;

                doorCollider = other.transform.GetChild(0).GetComponent<BoxCollider>();

                // 2�b���DisableCollider���\�b�h���Ăяo��
                Invoke("DisableCollider", 1f);
            }
        }
    }
    void UnlockDoor()
    {
        if(keysCollected)
        {
            for (int i = 0; i < collectedKeys; i++)
            {
                zyomaeLocks[i].SetActive(false);

                Rigidbody parentRigidbody = zyomaeLocks[i].transform.parent.GetComponent<Rigidbody>();
                if (parentRigidbody != null)
                {
                    parentRigidbody.isKinematic = false;
                }
            }
            audioSource_Lock.PlayOneShot(Lockse);
        }
    }


    void DisableCollider()
    {
        // BoxCollider�𖳌��ɂ���
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
    }
}
