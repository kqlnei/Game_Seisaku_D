using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooAudio : MonoBehaviour
{

    public AudioClip sound1;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // BehindAreaスクリプトを取得
        BehindArea behindAreaScript = GetComponentInParent<BehindArea>();

        // behindEnterがTrueになったら音(sound1)を鳴らす
        if (behindAreaScript.behindEnter == false)
        {
            audioSource.PlayOneShot(sound1);
            // ここで適切な処理を実行
            Debug.Log("BOOOO");
        }
    }
}
