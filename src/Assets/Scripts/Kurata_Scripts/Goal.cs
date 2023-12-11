using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Goal : MonoBehaviour
{
    public TMP_Text goalText;
    // ColliderがGoalエリアに入ったらtrueにするフラグ
    private bool goalEntered = false;
    public bool goal = true;

    public AudioClip goal_se;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

        // OnTriggerEnterはColliderが他のColliderに入った瞬間に呼ばれる
        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // もしくは他のタグを使用
        {
            // プレイヤーがGoalエリアに入ったらフラグをtrueに設定
            goalEntered = true;
            // ここで結果シーンに遷移するメソッドを呼び出す
            ShowGoalText();
            Invoke("Result", 5f);
            goal = false;
        }
    }

    private void Result()
    {
        // フラグがtrueの場合、結果シーンに遷移
        if (goalEntered)
        {
            SceneManager.LoadSceneAsync("ResultScene");
        }
    }

    private void ShowGoalText()
    {
        // goalTextが存在し、まだ表示されていない場合
        if (goalText != null && !goalText.gameObject.activeSelf)
        {
            goalText.gameObject.SetActive(true); // テキストを表示する

            audioSource.PlayOneShot(goal_se);
        }
    }
}
