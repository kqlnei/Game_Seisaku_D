using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScoreManagerScript : MonoBehaviour
{
    [Header("��public static���g�p")]
    [Header("�V�[���ԍ�")]
    [SerializeField] private int scene_Nam;

    public static int Scene_Namber;
    public static float Play_Time;
    public static int Player_Life;
   
    bool _Gole;

    [Header("�v���C���[�I�u�W�F�N�g")]
    [SerializeField] GameObject _Pleyer;
    LifeScript _Health;
    [Header("�S�[���I�u�W�F�N�g")]
    [SerializeField] GameObject _GoalObject;
    Goal _goal;
    // Start is called before the first frame update
    void Start()
    {
        Scene_Namber = scene_Nam;

        _Health = _Pleyer.GetComponent<LifeScript>();
        _goal = _GoalObject.GetComponent<Goal>();

        Play_Time = 0f;
        _Gole = true;
    }

    // Update is called once per frame
    void Update()
    {
        _Gole = _goal.goal;

        if (_Gole)
        {
            Play_Time = Play_Time + Time.deltaTime;
        }
        else
        {
            Player_Life = (int)_Health.TakeLife();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
