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
        // BehindArea�X�N���v�g���擾
        BehindArea behindAreaScript = GetComponentInParent<BehindArea>();

        // behindEnter��True�ɂȂ����特(sound1)��炷
        if (behindAreaScript.behindEnter == false)
        {
            audioSource.PlayOneShot(sound1);
            // �����œK�؂ȏ��������s
            Debug.Log("BOOOO");
        }
    }
}
