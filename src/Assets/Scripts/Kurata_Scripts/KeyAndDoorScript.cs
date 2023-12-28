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

    private int Lockse_sum = 0;

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
            // プレイヤーの持っている鍵を削除
            Destroy(other.gameObject);
            KeyImages[collectedKeys - 1].enabled = true;
            audioSource_Key.PlayOneShot(Keyse);
        }

        if (other.CompareTag("Door"))
        {
            // 錠前が外れる処理を実行
            UnlockDoor();

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            // すべての鍵を取得した場合、扉を開ける処理
            if (collectedKeys >= requiredKeys)
            {
                // 接触したオブジェクトの子オブジェクトを取得
                Transform childTransform = other.transform.GetChild(0);
                // 子オブジェクトにアタッチされているAnimatorコンポーネントを取得
                childAnimator = childTransform.GetComponent<Animator>();

                if (childAnimator != null)
                {
                    childAnimator.SetBool("DoorOpen", true);
                }

                Door_Open = true;

                doorCollider = other.transform.GetChild(0).GetComponent<BoxCollider>();

                // 2秒後にDisableColliderメソッドを呼び出す
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
            
            if(collectedKeys == Lockse_sum+1)
            {
                audioSource_Lock.PlayOneShot(Lockse);
                Lockse_sum++;
            }
        }
    }


    void DisableCollider()
    {
        // BoxColliderを無効にする
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
    }
}
