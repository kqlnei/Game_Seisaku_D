using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HaerdSoundsScript : MonoBehaviour
{
    [SerializeField] GameObject[] Enemy;

    [SerializeField] private float Distanse;
    EnemySearchScript[] Script;
    private float Interval = 1.0f;
    private float SetTime = 0f;
    public byte Chack;
    // Start is called before the first frame update
    void Start()
    {
        SetTime = 0f;
        Chack = 0;
        Script = new EnemySearchScript[Enemy.Length];

        for (int i = 0; i < Enemy.Length; i++)
        {
            Script[i] = Enemy[i].GetComponent<EnemySearchScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Chack == 1)
            SetTime += Time.deltaTime;
        if (Chack == 1 && SetTime < Interval)
            Fall();
        if(Chack == 1 && SetTime > Interval)
            Chack++;
    }

    private void Fall()
    {
        for (int i = 0; i < Enemy.Length; i++)
        {
            if (Enemy[i].activeSelf)
                if (Distanse > Vector3.Distance(Enemy[i].transform.position, this.transform.position))
                {
                    Script[i].EnemyMove = EnemySearchScript.Move.Heard;
                }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SetTime == 0f)
            Chack++;
    }
}
