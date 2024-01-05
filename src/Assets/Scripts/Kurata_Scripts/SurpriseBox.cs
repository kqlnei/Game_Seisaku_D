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
            // プレハブを生成
            Instantiate(Surprise_Box, transform.position, Quaternion.identity);
            canSpawn = false;
            StartCoroutine(StartCooldown());
        }
        if(!canSpawn)
        {
            // UIの表示状態を制御
            UIobj.enabled = true;  // デフォルトは表示

            UIobj.fillAmount -= 1.0f / cooldownTimer * Time.deltaTime;

            // UIobj.fillAmountが0より大きい場合はImageを表示（true）、そうでない場合は非表示（false）
            UIobj.enabled = UIobj.fillAmount > 0;
        }
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldownTimer);
        UIobj.fillAmount = 1;
        canSpawn = true;

        // カウントダウンが終了したのでImageを非表示にする
        UIobj.enabled = false;
    }
}
