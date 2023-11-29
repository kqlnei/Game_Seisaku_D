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

    // アニメーションイベントで呼び出されるメソッド
    public void startAnimation()
    {
        Debug.Log("Animation Event: startAnimation called!");
        // ここにアニメーションが開始された後に実行される処理を追加
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    public void endAnimation()
    {
        Debug.Log("Animation Event: endAnimation called!");
        // ここにアニメーションが開始された後に実行される処理を追加
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
