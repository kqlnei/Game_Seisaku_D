using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyVisibility : MonoBehaviour
{
    public bool visbilityEnter;
    //public GameObject escapetextObject;
    //[SerializeField] private GameObject player;
    //public bool TimeOvercontroll;
    //LifeScript _life;

    //public float Interval = 3.0f;
    //private float TimeCount = 0.0f;
    //public Slider slider;
    //private float originalSliderValue;
    //private Coroutine returnTo100Coroutine;

    // LifeSliderScript a;

    // Start is called before the first frame update
    void Start()
    {
        //TimeOvercontroll = true;
        visbilityEnter = false;
        //escapetextObject.SetActive(false);

         // TimeCount = 0.0f;

        //_life = player.GetComponent<LifeScript>();
        ///slider.value = 100;
        //originalSliderValue = slider.value;

        //hidetextComponent = HidetextObject.GetComponent<TextMeshProUGUI>();
    }
    
    // Update is called once per frame
    void Update()
    {
        /*if (visbilityEnter)
        {
             TimeCount = TimeCount + Time.deltaTime;

             if (TimeCount > Interval && TimeOvercontroll)
             {
                 Debug.Log(visbilityEnter);               

                 if (_life.currentHealth > 0)
                 {
                     TimeOvercontroll = false;
                     _life.TakeDamage(1);
                 }
             }
            float decreaseSpeed = 5f;
            float targetValue = slider.value - decreaseSpeed;

            slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * decreaseSpeed);

            originalSliderValue = slider.value;

            // プレイヤーがダメージを受けたらコルーチンを停止
            //StopCoroutine(returnTo100Coroutine);

            if (_life.currentHealth > 0)
            {
                if (slider.value < 1)
                {
                    TimeOvercontroll = false;
                    _life.TakeDamage(1);
                    slider.value = 100;
                }
            }
            else
            {
                slider.value = 0;
            }

        }
        //else TimeCount = 0.0f;
        */
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            visbilityEnter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            visbilityEnter = false;
            //TimeOvercontroll = true;
        }
    }
}
