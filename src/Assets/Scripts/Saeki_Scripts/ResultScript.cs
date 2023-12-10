using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class ResultScript : MonoBehaviour
{
    [Header("画像をぶち込む")]
    [SerializeField] public Image Next_Image;
    [SerializeField] public Image Restart_Image; 
    [SerializeField] public Image Serect_Image;

    [Header("選択中の色")]
    public Color32 Serect_Color; 

    [Header("非選択中の色")]
    public Color32 Not_Serect_Color;

    public int Stage_Number = ScoreManagerScript.Scene_Namber;

    private int Number;
    [Header("シーン名")]
    [SerializeField] string[] Scene_Name;

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
        Number = 0;
        Chenge();
        leftStickEnabled = true;
        rightStickEnabled = true;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        leftStickValue = Gamepad.current.leftStick.ReadValue().y;
        rightStickValue = Gamepad.current.rightStick.ReadValue().y;
        if (Gamepad.current.aButton.wasPressedThisFrame) //Input.GetKeyDown(KeyCode.Space)
        {
            GetSE();

            switch (Number)
            {
                case 0:
                    {
                        if(Scene_Name.Length > Stage_Number + 1)
                            SceneManager.LoadSceneAsync(Scene_Name[Stage_Number + 1]);
                    }
                    break;
                case 1:
                    {
                        SceneManager.LoadSceneAsync(Scene_Name[Stage_Number]);
                    }
                    break;
                case 2:
                    {
                        SceneManager.LoadSceneAsync(Scene_Name[0]);
                    }
                    break;
            }
        }

        else if (Gamepad.current.dpad.up.wasPressedThisFrame || leftStickValue > 0.6f ||rightStickValue > 0.6f) //Input.GetKeyDown(KeyCode.UpArrow)
        {
            if(leftStickEnabled && rightStickEnabled)
            {
                Number--;
                if (Number < 0)
                    Number = 0;
                else
                    GetSE();

                Chenge();
                leftStickEnabled = false;
                rightStickEnabled = false;
            }
        }

        else if (Gamepad.current.dpad.down.wasPressedThisFrame || leftStickValue < -0.6f || rightStickValue < -0.6f) //Input.GetKeyDown(KeyCode.DownArrow)
        {
            if (leftStickEnabled && rightStickEnabled)
            {
                Number++;
                if (Number > 2)
                    Number = 2;
                else
                    GetSE();
                Chenge();
                leftStickEnabled = false;
                rightStickEnabled = false;
            }
        }

        NeutralCheck();
    }

    void GetSE()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(sound1);
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

    void Chenge()
    {
        switch (Number)
        {
            case 0:
                {
                    Next_Image.color = Serect_Color;
                    Restart_Image.color = Not_Serect_Color;
                    Serect_Image.color = Not_Serect_Color;
                }
                break;
            case 1:
                {
                    Next_Image.color = Not_Serect_Color;
                    Restart_Image.color = Serect_Color;
                    Serect_Image.color = Not_Serect_Color;
                }
                break;
            case 2:
                {
                    Next_Image.color = Not_Serect_Color;
                    Restart_Image.color = Not_Serect_Color;
                    Serect_Image.color = Serect_Color;
                }
                break;
        }
    }
}
