using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class BehindArea : MonoBehaviour
{
    
    public Image image;
    private bool playerInsideArea = false;

    public bool behindEnter;

    Animator m_Animator;
    private GameObject playerObject;
    private Playercontroller playerController;

    public AudioClip sound1, sound2;
    AudioSource audioSource;

    public Image ButtonPushImage;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            m_Animator = playerObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Player object not found. Make sure it has the 'Player' tag.");
        }
        ButtonPushImage.gameObject.SetActive(false);
        behindEnter = false;
        playerController = GetComponent<Playercontroller>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Z�L�[�������ꂽ�Ƃ�
        if (Gamepad.current.buttonEast.wasReleasedThisFrame && playerInsideArea)// || (Input.GetKeyDown(KeyCode.Z) && playerInsideArea)
        {
            m_Animator.SetBool("Pow", true);
            Invoke("ResetPowParameter", 3f);
            playerInsideArea = false;
            behindEnter = true;
            // ImageUI��\������
            image.gameObject.SetActive(true);
            Invoke("PowTextFalse", 0.5f);
            // �L�[�������ꂽ�Ƃ��APlayerTag���t�����I�u�W�F�N�g��e�I�u�W�F�N�g�̕����Ɍ�����
            RotatePlayerTowardsParent();

            audioSource.PlayOneShot(sound1);
            audioSource.PlayOneShot(sound2);
            Debug.Log("BOOOO");
        }
    }

    void RotatePlayerTowardsParent()
    {
        if (playerObject != null && playerObject.CompareTag("Player"))
        {
            // �e�I�u�W�F�N�g���擾
            Transform parentTransform = transform.parent;

            if (parentTransform != null)
            {
                // �e�I�u�W�F�N�g�̕������擾���Ay�����������l�����ăv���C���[�I�u�W�F�N�g��������
                Vector3 directionToParent = parentTransform.position - playerObject.transform.position;
                directionToParent.y = 0f; // y�������𖳎�
                Quaternion lookRotation = Quaternion.LookRotation(directionToParent);
                playerObject.transform.rotation = lookRotation;
            }
            else
            {
                Debug.LogError("Parent transform not found.");
            }
        }
    }
    

    void ResetPowParameter()
    {
        m_Animator.SetBool("Pow", false);
    }

    void PowTextFalse()
    {
        ButtonPushImage.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // behindEnter = true; Debug.Log(behindEnter);
            playerInsideArea = true;
            ButtonPushImage.gameObject.SetActive(true);
            image.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideArea = false;
            ButtonPushImage.gameObject.SetActive(false);
            behindEnter = false;
        }
    }

    void OnDestroy()
    {
        // �I�u�W�F�N�g���j�������Ƃ��Ɏ��s����鏈��
        playerInsideArea = false;
        ButtonPushImage.gameObject.SetActive(false);
        behindEnter = false;
        Debug.Log("OnDestroy");
    }
}
