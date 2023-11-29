using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEnd : MonoBehaviour
{
    private Playercontroller playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<Playercontroller>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �A�j���[�V�����C�x���g�ŌĂяo����郁�\�b�h
    public void startAnimation()
    {
        Debug.Log("Animation Event: startAnimation called!");
        // �����ɃA�j���[�V�������J�n���ꂽ��Ɏ��s����鏈����ǉ�
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    public void endAnimation()
    {
        Debug.Log("Animation Event: endAnimation called!");
        // �����ɃA�j���[�V�������J�n���ꂽ��Ɏ��s����鏈����ǉ�
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
