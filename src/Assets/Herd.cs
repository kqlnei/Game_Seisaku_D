using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour
{
    [SerializeField]Color32 color;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        this.gameObject.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
