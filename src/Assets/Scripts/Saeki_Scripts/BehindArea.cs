using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class BehindArea : MonoBehaviour
{
    public GameObject textObject;
    public Image image;
    private bool playerInsideArea = false;

    public bool behindEnter;

    Animator m_Animator;
    private GameObject playerObject;
    private Playercontroller playerController;

    public AudioClip sound1, sound2;
    AudioSource audioSource;

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
        textObject.SetActive(false);
        behindEnter = false;
        playerController = GetComponent<Playercontroller>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Zキーが押されたとき
        if (Gamepad.current.buttonEast.wasReleasedThisFrame && playerInsideArea)// || (Input.GetKeyDown(KeyCode.Z) && playerInsideArea)
        {
            m_Animator.SetBool("Pow", true);
            Invoke("ResetPowParameter", 3f);
            playerInsideArea = false;
            behindEnter = true;
            // ImageUIを表示する
            image.gameObject.SetActive(true);
            Invoke("PowTextFalse", 0.5f);
            // キーが押されたとき、PlayerTagが付いたオブジェクトを親オブジェクトの方向に向ける
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
            // 親オブジェクトを取得
            Transform parentTransform = transform.parent;

            if (parentTransform != null)
            {
                // 親オブジェクトの方向を取得し、y軸方向だけ考慮してプレイヤーオブジェクトを向ける
                Vector3 directionToParent = parentTransform.position - playerObject.transform.position;
                directionToParent.y = 0f; // y軸方向を無視
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
        textObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // behindEnter = true; Debug.Log(behindEnter);
            playerInsideArea = true;
            textObject.SetActive(true);
            image.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideArea = false;
            textObject.SetActive(false);
            behindEnter = false;
        }
    }
}
