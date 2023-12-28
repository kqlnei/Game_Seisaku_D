using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeSliderScript : MonoBehaviour
{
    [SerializeField] private GameObject[] Light;
    private EnemyLight[] Lig;

    [SerializeField] public Slider slider;

    public Image escapeImage;

    [SerializeField] GameObject player;
    LifeScript _life;

    private float originalSliderValue;
    private float Interval = 2.0f;
    private float Timecount = 0f;

    private bool Damaged;


    public AudioClip damage;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Lig = new EnemyLight[Light.Length];

        for (int i = 0; i < Lig.Length; i++)
            Lig[i] = Light[i].GetComponent<EnemyLight>();

        slider.value = 100;
        originalSliderValue = slider.value;
        escapeImage.gameObject.SetActive(false);
        Damaged = false;

        _life = player.GetComponent<LifeScript>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool AllEnter = false;

        for (int i = 0; i < Light.Length; i++)
            if(Light[i].activeSelf)
                if(Lig[i].lightEnter) 
                { 
                    AllEnter = true; break; 
                }

        originalSliderValue = slider.value;

        if (AllEnter)
        {
            escapeImage.gameObject.SetActive(true);
            Timecount += Time.deltaTime;

            float decreaseSpeed = 6f;
            float targetValue = slider.value - decreaseSpeed;

            if (Damaged)
            {
                if (Timecount > Interval)
                    Damaged = false;
            }
            else
                slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * decreaseSpeed);
            Debug.Log(slider.value);
            if (_life.currentHealth > 0)
            {
                if (slider.value < 1)
                {
                    for(int i = 0; i < Light.Length; i++)
                        if (Light[i].activeSelf)
                            if (Lig[i].lightEnter)
                                Lig[i].TimeOvercontroll = false;

                    _life.TakeDamage(1);
                    //Debug.Log(Timecount);
                    audioSource.PlayOneShot(damage);

                    slider.value = 100;

                    Damaged = true;
                    Timecount = 0f;
                }
            }
            else
            {
                slider.value = 0;
            }
        }

        else
        {
            Damaged = false;
            Timecount = 0f;
            escapeImage.gameObject.SetActive(false);

            if (slider.value < 1)
                slider.value = 0;

            //else if (slider.value < 100)
                //slider.value = Mathf.Lerp(slider.value, 100, 0.001f);
        }
    }
}
