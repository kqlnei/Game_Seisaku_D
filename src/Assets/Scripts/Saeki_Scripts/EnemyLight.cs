using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLight : MonoBehaviour
{
    public bool lightEnter;
    public bool TimeOvercontroll;

    public AudioClip chase, sound2;
    AudioSource audioSource;
    //public Slider slider;

    //private float originalSliderValue;

    //private Coroutine returnTo100Coroutine;
    // Start is called before the first frame update
    void Start()
    {
        TimeOvercontroll = true;
        lightEnter = false;
        audioSource = GetComponent<AudioSource>();
        //slider.value = 100;
        //originalSliderValue = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
              
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightEnter = true;
            audioSource.PlayOneShot(chase);
            /*float decreaseSpeed = 5f;
            float targetValue = slider.value - decreaseSpeed;

            slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * decreaseSpeed);

            originalSliderValue = slider.value;

            // プレイヤーがダメージを受けたらコルーチンを停止
            StopCoroutine(returnTo100Coroutine);

            if (slider.value < 1)
            {
                lightEnter = true;
                LifeScript life = other.GetComponent<LifeScript>();
                if (life != null)
                {
                    life.TakeDamage(1);
                }
                slider.value = 100;
               
            }*/
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightEnter = false;
            TimeOvercontroll = true;
            audioSource.Stop();
            // コルーチンを開始し、そのCoroutineオブジェクトへの参照を保持
            //returnTo100Coroutine = StartCoroutine(ReturnSliderTo100OverTime());
        }
    }

    /*IEnumerator ReturnSliderTo100OverTime()
    {
        float elapsedTime = 0f;
        float duration = 10f;

        while (elapsedTime < duration)
        {
            slider.value = Mathf.Lerp(originalSliderValue, 100, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        slider.value = 100;
    }*/
}
