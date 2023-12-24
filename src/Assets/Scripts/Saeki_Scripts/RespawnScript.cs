using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField] GameObject[] Enemis;
    [SerializeField] GameObject _Player;
    [SerializeField] float _Interbal = 30f;
    private float[] RespawnTime;
    private Vector3[] Points;
    [SerializeField] float _distance = 3f;
    // Start is called before the first frame update
    void Start()
    {
        RespawnTime = new float[Enemis.Length];
        Points = new Vector3[Enemis.Length];
        for (int i = 0; i < Enemis.Length; i++)
            RespawnTime[i] = 0f;

        if (SecretComandScript.Comand)
        {
            _distance = 0.1f;
            _Interbal = 7f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < Enemis.Length; i++)
        {
            if(!Enemis[i].activeSelf)
            {
                RespawnTime[i] = RespawnTime[i] + Time.deltaTime;

                if (RespawnTime[i] >= _Interbal)
                {
                    float dis = Vector3.Distance(_Player.transform.position, Points[i]);

                    if(dis >= _distance)
                        EnemyActive(i);
                }
            }

            else
            {
                Points[i] = Enemis[i].transform.position;
            }
        }
    }

    void EnemyActive(int num)
    {
        Enemis[num].SetActive(true);
        RespawnTime[num] = 0f;
    }
}
