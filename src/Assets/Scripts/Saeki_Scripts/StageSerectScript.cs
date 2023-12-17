using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class StageSerectScript : MonoBehaviour
{
    [Header("スタート画像")]
    [SerializeField] GameObject Start_Image;
    [Header("オプション画像")]
    [SerializeField] GameObject Option_Image;

    [Space]
    [Header("ステージ画像")]
    [SerializeField] GameObject Stage_Image;
    [Space]
   
    [Header("チュートリアル画像")]
    [SerializeField] GameObject Tutorial_Image;
    [Header("セカンドステージ画像")]
    [SerializeField] GameObject Serect_Stege_Image;

    [Space]
    [Header("選ぶ場所の数")]
    public int Serectpoints = 2;

    [Space]
    [Header("ステージの数")]
    public int Stagepoints = 2;

    [Space]
    [Header("縦ずれ修正")]
    public float Vertical = 50f; 
    [Space]
    [Header("横ずれ修正")]
    public float Horizontal = -100f;

    int[] SerectControll = new int[2];
    int SerectComand;

    bool EnterImages = false;

    private float leftStickValue;
    private float rightStickValue;
    private bool leftStickEnabled;
    private bool rightStickEnabled;

    private float NeutralValue;

    public AudioClip sound1;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        NeutralValue = 0.15f;
        MoveImage(Start_Image);
        SerectControll[0] = 0;
        SerectControll[1] = 0;
        SerectComand = 0;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        leftStickValue = Gamepad.current.leftStick.ReadValue().y;
        rightStickValue = Gamepad.current.rightStick.ReadValue().y;
        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            GetSerect();
            GetSE();
        }

        else if (Gamepad.current.dpad.left.wasPressedThisFrame || Gamepad.current.xButton.wasPressedThisFrame)
        {
            SerectComand--;
            if (SerectComand < 0)
                SerectComand++;

            SerectControll[SerectComand] = 0;

            GetImage(SerectControll[SerectComand]);

            if (EnterImages)
            {
                EnterImages = false;
                EnterImage(EnterImages);
            }
            GetSE();
        }

        else if (Gamepad.current.dpad.up.wasPressedThisFrame || leftStickValue > 0.6f || rightStickValue > 0.6f) //Input.GetKeyDown(KeyCode.UpArrow)
        {
            if (leftStickEnabled && rightStickEnabled)
            {
                leftStickEnabled = false;
                rightStickEnabled = false;
                SerectControll[SerectComand]--;

                if (SerectControll[SerectComand] < 0)
                    SerectControll[SerectComand] = 0;

                else
                    GetSE();

                GetImage(SerectControll[SerectComand]);
               
            }
        }

        else if (Gamepad.current.dpad.down.wasPressedThisFrame || leftStickValue < -0.6f || rightStickValue < -0.6f) //Input.GetKeyDown(KeyCode.DownArrow)
        {
            if (leftStickEnabled && rightStickEnabled)
            {
                leftStickEnabled = false;
                rightStickEnabled = false;
                SerectControll[SerectComand]++;

                if (SerectControll[SerectComand] >= Serectpoints)
                    SerectControll[SerectComand] = Serectpoints - 1;

                else
                    GetSE();

                GetImage(SerectControll[SerectComand]);
                
            }    
        }

        NeutralCheck();

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(SerectControll[SerectComand]); 
            Debug.Log(SerectComand);
            GetSE();
        }
    }

    void GetSE()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(sound1);
    }

    void GetImage(int Serect)
    {
        if(SerectComand == 0) 
        {
            if (Serect == 0)
            {
                MoveImage(Start_Image);
            }
            if (Serect == 1)
            {
                MoveImage(Option_Image);
            }
        }
        if (SerectComand == 1)
        {
            if (Serect == 0)
            {
                MoveImage(Tutorial_Image);
            }
            if (Serect == 1)
            {
                MoveImage(Serect_Stege_Image);
            }
        }
    }

    void MoveImage(GameObject Image)
    {
        transform.position = Image.transform.position;
        this.transform.position += new Vector3(Horizontal, Vertical, 0);
    }

    void EnterImage(bool get)
    {
        Tutorial_Image.SetActive(get);
        Serect_Stege_Image.SetActive(get);
        Stage_Image.SetActive(get);
    }

    void OpenOpsion()
    {

    }

    void GetSerect()
    {
        if (SerectComand == 1)
        {
            if (SerectControll[SerectComand] == 0)
            {
                SceneManager.LoadSceneAsync("Tutorial_Stage");
            }

            else if (SerectControll[SerectComand] == 1)
            {
                SceneManager.LoadSceneAsync("Second_Stage");
            }
        }

        if (SerectComand == 0)
        {
            if (SerectControll[SerectComand] == 0)
            {
                SerectComand++;
                if (SerectComand > 1)
                    SerectComand--;

                if (!EnterImages)
                {
                    EnterImages = true;
                    EnterImage(EnterImages);
                }
                   
 
                    GetImage(SerectControll[SerectComand]);
            }

            else if (SerectControll[SerectComand] == 1)
            {
                OpenOpsion();
            }
        }
    }

    void NeutralCheck()
    {
        if (leftStickValue < NeutralValue && leftStickValue > -NeutralValue)
        {
            leftStickEnabled = true;
        }

        if (rightStickValue < NeutralValue && rightStickValue > -NeutralValue)
        {
            rightStickEnabled = true;
        }
    }
}
