using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeSliderScript : MonoBehaviour
{
    [SerializeField] private GameObject[] Visibility;
    private EnemyVisibility[] Vis;

    [SerializeField] private Slider slider;

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
        Vis = new EnemyVisibility[Visibility.Length];

        for (int i = 0; i < Vis.Length; i++)
            Vis[i] = Visibility[i].GetComponent<EnemyVisibility>();

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

        for (int i = 0; i < Visibility.Length; i++)
            if(Visibility[i].activeSelf)
                if(Vis[i].visbilityEnter) 
                { 
                    AllEnter = true; break; 
                }

        originalSliderValue = slider.value;

        if (AllEnter)
        {
            escapeImage.gameObject.SetActive(true);
            Timecount += Time.deltaTime;

            float decreaseSpeed = 5f;
            float targetValue = slider.value - decreaseSpeed;

            if (Damaged)
            {
                if (Timecount > Interval)
                    Damaged = false;
            }
            else
                slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * decreaseSpeed);

            if (_life.currentHealth > 0)
            {
                if (slider.value < 1)
                {
                    for(int i = 0; i < Visibility.Length; i++)
                        if (Visibility[i].activeSelf)
                            if (Vis[i].visbilityEnter)
                                Vis[i].TimeOvercontroll = false;

                    _life.TakeDamage(1);


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

            if (slider.value < 100)
                slider.value = Mathf.Lerp(slider.value, 100, 0.001f);
            
            else
                slider.value = 100;
        }

    }
}
