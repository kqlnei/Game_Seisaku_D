using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour
{
    //[SerializeField]Color32 color;
    [SerializeField] private Material Mat_Y;
    [SerializeField] private Material Mat_R;
    //[SerializeField] private GameObject Enemy;
    private bool Mat_Change;
    [SerializeField] GameObject Visbility;
    EnemyVisibility _VisbilityTrigger;
    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.transform.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        //this.gameObject.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

        _VisbilityTrigger = Visbility.GetComponent<EnemyVisibility>();
        Mat_Change = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_VisbilityTrigger.visbilityEnter && !Mat_Change)
        {
            this.GetComponent<MeshRenderer>().material = Mat_R;
            Mat_Change = true;
        }


        else if (!_VisbilityTrigger.visbilityEnter && Mat_Change)
        {
            this.GetComponent<MeshRenderer>().material = Mat_Y;
            Mat_Change = false;
        }
    }
}
