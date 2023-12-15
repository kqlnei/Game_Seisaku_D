using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class ResultScript : MonoBehaviour
{
    [Header("画像をぶち込む")]
    [SerializeField] private Image Next_Image;
    [SerializeField] private Image Restart_Image; 
    [SerializeField] private Image Serect_Image;
    [Space]
    [SerializeField] private Image Next_BrackImage;
    [SerializeField] private Image Restart_BrackImage;
    [SerializeField] private Image Serect_BrackImage;

    [Header("選択中の色")]
    [SerializeField] private Color32 Serect_Color; 

    [Header("非選択中の色")]
    [SerializeField] private Color32 Not_Serect_Color;

    private Color32 Not_Clear_Color = new(0, 0, 0, 127);
    private Color32 Clear_Color = new(0,0,0,0);

    [Space]
    public int Stage_Number = ScoreManagerScript.Scene_Namber;

    private Vector3 Def = new Vector3(1f, 1f, 1f);
    private Vector3 Set = new Vector3(1.2f, 1.2f, 1f);

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
        Next_Image.rectTransform.localScale = Set;
        Restart_BrackImage.color = Not_Clear_Color;
        Serect_BrackImage.color = Not_Clear_Color;

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

                    Next_Image.rectTransform.localScale = Set;
                    Restart_Image.rectTransform.localScale = Def;
                    Serect_Image.rectTransform.localScale = Def;

                    Next_BrackImage.color = Clear_Color;
                    Restart_BrackImage.color = Not_Clear_Color;
                    Serect_BrackImage.color = Not_Clear_Color;
                }
                break;
            case 1:
                {
                    Next_Image.color = Not_Serect_Color;
                    Restart_Image.color = Serect_Color;
                    Serect_Image.color = Not_Serect_Color;

                    Next_Image.rectTransform.localScale = Def;
                    Restart_Image.rectTransform.localScale = Set;
                    Serect_Image.rectTransform.localScale = Def;

                    Next_BrackImage.color = Not_Clear_Color;
                    Restart_BrackImage.color = Clear_Color;
                    Serect_BrackImage.color = Not_Clear_Color;
                }
                break;
            case 2:
                {
                    Next_Image.color = Not_Serect_Color;
                    Restart_Image.color = Not_Serect_Color;
                    Serect_Image.color = Serect_Color;

                    Next_Image.rectTransform.localScale = Def;
                    Restart_Image.rectTransform.localScale = Def;
                    Serect_Image.rectTransform.localScale = Set;

                    Next_BrackImage.color = Not_Clear_Color;
                    Restart_BrackImage.color = Not_Clear_Color;
                    Serect_BrackImage.color = Clear_Color;
                }
                break;
        }
    }
}
