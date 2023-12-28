using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Goal : MonoBehaviour
{
    public Image goalImage;
    // ColliderがGoalエリアに入ったらtrueにするフラグ
    private bool goalEntered = false;
    public bool goal = true;

    public AudioClip goal_se;
    private AudioSource audioSource;

    private float triggerDelay = 2f; // トリガーを設定するまでの待機時間

    public CinemachineVirtualCameraBase vcam3;

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

            DisablePlayerController(other);  // プレイヤーコントローラーを無効化するメソッドを呼び出す
            StartCoroutine(MovePlayerToTarget(other));// プレイヤーを目標地点まで移動させるメソッドを呼び出す

            StartCoroutine(SetAnimatorTriggerWithDelay(other, triggerDelay));


            Invoke("Result", 7f);
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
        if (goalImage != null && !goalImage.gameObject.activeSelf)
        {
            goalImage.gameObject.SetActive(true); // テキストを表示する

            audioSource.PlayOneShot(goal_se);
        }
    }

    private IEnumerator MovePlayerToTarget(Collider other)
    {
        Transform targetTransform = transform.GetChild(0); // 0は子オブジェクトのインデックス。必要に応じて変更
        if (targetTransform != null)
        {
            Vector3 targetPosition = targetTransform.position;

            float duration = 3f; // 移動にかかる時間
            float elapsedTime = 0f;

            Vector3 initialPosition = other.transform.position;

            vcam3.Priority = 5;

            while (elapsedTime < duration)
            {
                other.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
                other.transform.LookAt(targetPosition); // 目的地の方向を向く

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 最終地点に180度回転
            other.transform.Rotate(0f, 180f, 0f);

           
        }
    }

    private void DisablePlayerController(Collider other)
    {
        // プレイヤーコントローラースクリプトを無効化する
        Playercontroller playerController = other.GetComponent<Playercontroller>();

        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    private IEnumerator SetAnimatorTriggerWithDelay(Collider other, float delay)
    { 
        yield return new WaitForSeconds(delay);

        // AnimatorのClearTriggerをTrueに設定する
        Animator animator = other.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Clear", true);
        }
    }
}
