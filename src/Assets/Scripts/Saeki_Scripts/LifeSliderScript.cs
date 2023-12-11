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

    public GameObject escapetextObject;

    [SerializeField] GameObject player;
    LifeScript _life;

    private float originalSliderValue;
    private float Interval = 2.0f;
    private float Timecount = 0f;

    private bool Damaged;

    // Start is called before the first frame update
    void Start()
    {
        Vis = new EnemyVisibility[Visibility.Length];

        for (int i = 0; i < Vis.Length; i++)
            Vis[i] = Visibility[i].GetComponent<EnemyVisibility>();

        slider.value = 100;
        originalSliderValue = slider.value;
        escapetextObject.SetActive(false);
        Damaged = false;

        _life = player.GetComponent<LifeScript>();
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
            escapetextObject.SetActive(true);
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
            escapetextObject.SetActive(false);

            if (slider.value < 100)
                slider.value = Mathf.Lerp(slider.value, 100, 0.001f);
            
            else
                slider.value = 100;
        }

    }
}
