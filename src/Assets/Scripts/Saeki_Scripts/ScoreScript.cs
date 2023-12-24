using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _Time;
    [SerializeField] TextMeshProUGUI _Life;
    [SerializeField] TextMeshProUGUI _Grade;

    float time = ScoreManagerScript.Play_Time;
    int life = ScoreManagerScript.Player_Life;

    int Stege_Number = ScoreManagerScript.Scene_Namber;

    [Header("GRADE‚ÌŠî€’l(0.1`)")]
    public float Citerion_Time = 60f;

    private string grade = "NEDCBAS";//0`6
    void Start()
    {
        Set();
    }

    void Set()
    {
        float Cast_num1 = time * 100;
        int Cast_num2 = (int)Cast_num1;
        float Cast_num3 = (float)Cast_num2;
        _Time.text = " " +Cast_num3 / 100;
        _Life.text = " " + life;
        _Grade.text = grade.Substring(Result(), 1);//H•¶Žš–Ú‚©‚çH•¶Žš•ªŽæ“¾
    }

    int Result()
    {
        if (life == 0)
            return 0;

        float Summation = time * ((200f - (float)life) / 100f);

        float _Score = Summation / Citerion_Time;

        int _result = 0;

        for (int i = 6; i > 0; i--)
        {
            _result = i;

            if (_Score < 3f * Stege_Number + (5f - (float)i ))
             break;           
        }
        
        return _result;
    }




    void Update()
    {
      
    }
}
