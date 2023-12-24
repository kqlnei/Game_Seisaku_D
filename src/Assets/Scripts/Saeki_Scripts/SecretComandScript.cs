using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SecretComandScript : MonoBehaviour
{
    private float leftStickValue_x;
    private float rightStickValue_x;
    private float leftStickValue_y;
    private float rightStickValue_y;
    private bool leftStickEnabled;
    private bool rightStickEnabled;
    private float NeutralValue;

    private int SecretInput;

    public static bool Comand;

    // Start is called before the first frame update
    void Start()
    {
        NeutralValue = 0.15f;
        SecretInput = 0;
        Comand  = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 999, 0));

        leftStickValue_x = Gamepad.current.leftStick.ReadValue().x;
        rightStickValue_x = Gamepad.current.rightStick.ReadValue().x;
        leftStickValue_y = Gamepad.current.leftStick.ReadValue().y;
        rightStickValue_y = Gamepad.current.rightStick.ReadValue().y;

        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            if (SecretInput == 9)
            {
                SecretChange();
                SecretInput = 0;
            }
                
            else
                SecretInput = 0;
        }

        else if (Gamepad.current.bButton.wasPressedThisFrame)
        {
            if (SecretInput == 8)
                SecretInput++;
            else
                SecretInput = 0;
        }

        else if (Gamepad.current.dpad.up.wasPressedThisFrame || leftStickValue_y > 0.6f || rightStickValue_y > 0.6f)
        {
            if (leftStickEnabled && rightStickEnabled)
            {
                leftStickEnabled = false;
                rightStickEnabled = false;

                if (SecretInput == 0 || SecretInput == 1)
                    SecretInput++;
                else
                    SecretInput = 0;
            }
        }

        else if (Gamepad.current.dpad.down.wasPressedThisFrame || leftStickValue_y < -0.6f || rightStickValue_y < -0.6f)
        {
            if (leftStickEnabled && rightStickEnabled)
            {
                leftStickEnabled = false;
                rightStickEnabled = false;

                if (SecretInput == 2 || SecretInput == 3)
                    SecretInput++;
                else
                    SecretInput = 0;
            }
        }

        else if (Gamepad.current.dpad.left.wasPressedThisFrame || leftStickValue_x < -0.6f || rightStickValue_x < -0.6f)
        {
            if (leftStickEnabled && rightStickEnabled)
            {
                leftStickEnabled = false;
                rightStickEnabled = false;

                if (SecretInput == 4 || SecretInput == 6)
                    SecretInput++;
                else
                    SecretInput = 0;
            }
        }

        else if (Gamepad.current.dpad.right.wasPressedThisFrame || leftStickValue_x > 0.6f || rightStickValue_x > 0.6f)
        {
            if (leftStickEnabled && rightStickEnabled)
            {
                leftStickEnabled = false;
                rightStickEnabled = false;

                if (SecretInput == 5 || SecretInput == 7)
                    SecretInput++;
                else
                    SecretInput = 0;
            }
        }

        NeutralCheck();
    }

    void NeutralCheck()
    {
        if (leftStickValue_x < NeutralValue && leftStickValue_x > -NeutralValue)
            if (leftStickValue_y < NeutralValue && leftStickValue_y > -NeutralValue)
                leftStickEnabled = true;

        if (rightStickValue_x < NeutralValue && rightStickValue_x > -NeutralValue)
            if (rightStickValue_y < NeutralValue && rightStickValue_y > -NeutralValue)
                rightStickEnabled = true;
        
    }
    void SecretChange()
    {
        if (!Comand)
        {
            Comand = true;
            transform.position = new Vector3(7f, 0f, 1f);
        }
        else
        {
            Comand = false;
            transform.position = new Vector3(13f, 0f, 1f);
        }
    }
}
