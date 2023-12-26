using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleSceneScript : MonoBehaviour
{
    //===== ��`�̈� =====
    private Animator anim;  //Animator��anim�Ƃ����ϐ��Œ�`����

    private bool Put = false;
    private float Interval;
    public float ChangeTime = 1.6f;

    [SerializeField]private GameObject Image;

    public AudioClip sound1;
    AudioSource audioSource;
    
    private InputAction _pressAnyKeyAction = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");
   
    private void OnEnable() => _pressAnyKeyAction.Enable();
    private void OnDisable() => _pressAnyKeyAction.Disable();
    //===== �������� =====
    void Start()
    {
        //�ϐ�anim�ɁAAnimator�R���|�[�l���g��ݒ肷��
        anim = gameObject.GetComponent<Animator>();
        Put = false;
        Interval = 0.0f;
        Image.SetActive(true);
        audioSource = GetComponent<AudioSource>();
    }

    //===== �又�� =====
    void Update()
    {
        if (_pressAnyKeyAction.triggered && !Put)
        {
            audioSource.PlayOneShot(sound1);
            anim.SetTrigger("GetAnyKey");
            Put = true;
            Image.SetActive(false);
            
        }

        else if(Put)
        {
            Interval += Time.deltaTime;
        }

        if(Interval > ChangeTime)
        {
            SceneManager.LoadSceneAsync("SelectScene");
        }
    }
}
