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

    // 一度だけ音が再生されたかどうかを管理するフラグ
    private bool hasPlayedSound = false;

    void OnTriggerStay(Collider other)
    {
        // プレイヤータグのオブジェクトに触れているかどうか確認
        if (other.CompareTag("Player"))
        {
            // PoltergeisScriptのインスタンスを取得
            PoltergeisScript poltergeisScript = other.GetComponentInParent<PoltergeisScript>();

            // poltergeisScriptがnullでないか確認
            if (poltergeisScript != null)
            {
                // PoltergeisScriptのglass_break変数にアクセス
                bool isGlassBroken = poltergeisScript.glass_break;

                // glass_breakがTrueになり、かつまだ音が再生されていない場合
                if (isGlassBroken && !hasPlayedSound)
                {
                    // コルーチンを開始して音を再生
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
        // 数秒待ってから音を再生
        yield return new WaitForSeconds(0.5f); // 3秒待つ例

        // AudioSourceを取得して音を再生
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
            hasPlayedSound = true; // 音が再生されたフラグをセット
        }
    }
}
