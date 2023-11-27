using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class HideScript : MonoBehaviour
{
    public GameObject HidetextObject;
    private bool isPlayerInside = false;
    private Transform InSidePoint;
    private Transform OutSidePoint;
    private Transform nowPoint;
    bool HideMode = false;

    [SerializeField] float moveSpeed = 5.0f;

    private Playercontroller playerController;
    private BoxCollider boxCollider;

    private TextMeshProUGUI hidetextComponent;

    private CapsuleCollider capsuleCollider;

    private bool isMoving = false; // 移動中のフラグ
    // Start is called before the first frame update
    void Start()
    {
        HidetextObject.SetActive(false);
        hidetextComponent = HidetextObject.GetComponent<TextMeshProUGUI>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerController = GetComponent<Playercontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInside && !isMoving)
        {
            // プレイヤーがエリア内にいる場合の処理
            if (Gamepad.current.buttonSouth.wasReleasedThisFrame)// || Input.GetKeyDown(KeyCode.Z)
            {
                HideMode = !HideMode;


                ChangeHide();
            }

        }
    }

    void ChangeHide()
    {
        if (HideMode)
        {
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            if (boxCollider != null)
            {
                boxCollider.isTrigger = true;
            }
            capsuleCollider.enabled = false;
            hidetextComponent.text = "Press [A] to stop hiding";
            StartCoroutine(MoveToTarget(nowPoint.position, InSidePoint.position));

        }
        else
        {
           
            if (boxCollider != null)
            {
                boxCollider.isTrigger = false;
            }
            capsuleCollider.enabled = true;
            hidetextComponent.text = "Press [A] button";
            StartCoroutine(MoveToTarget(nowPoint.position, OutSidePoint.position));
        

        }
    }

    // MoveToTarget メソッド
    IEnumerator MoveToTarget(Vector3 nowPosition, Vector3 targetPosition)
    {
        targetPosition.y = 2f; // y座標を固定する
        isMoving = true; // 移動中のフラグをセット

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.LookAt(targetPosition);
            transform.position = Vector3.Slerp(nowPosition, targetPosition, moveSpeed * Time.deltaTime);
            nowPosition = transform.position;

            yield return null; // 1フレーム待機
        }

        transform.position = targetPosition; // 最終的な位置を目標の位置に設定
        isMoving = false; // 移動が終わったらフラグをリセット

        // 移動が完了したらプレイヤーコントローラーを有効にする
        if (!HideMode)
        {
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
                    
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Hide")
        {
            Transform parentTransform = col.transform.parent;
            boxCollider = parentTransform.GetComponent<BoxCollider>();

            isPlayerInside = true;
            HidetextObject.SetActive(true);
            
            OutSidePoint = col.transform.GetChild(0);
            InSidePoint = col.transform.GetChild(1);
            nowPoint = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        HidetextObject.SetActive(false);
    }
}