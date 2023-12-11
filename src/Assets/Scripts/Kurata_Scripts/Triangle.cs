using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    public float moveSpeed = 2.0f;  // �I�u�W�F�N�g�̏㉺�ړ����x
    public float moveDistance = 1.0f;  // �I�u�W�F�N�g���㉺���鋗��

    private Vector3 initialPosition;
    private float minY;
    private float maxY;
    private bool movingUp = true;
    [SerializeField] Color32 color;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        minY = initialPosition.y;
        maxY = initialPosition.y + moveDistance;
        this.gameObject.transform.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        this.gameObject.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }

    // Update is called once per frame
    void Update()
    {
        float newY = transform.position.y + (movingUp ? moveSpeed : -moveSpeed) * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // �ړ�������؂�ւ���
        if (transform.position.y >= maxY)
        {
            movingUp = false;
        }
        else if (transform.position.y <= minY)
        {
            movingUp = true;
        }
    }
}
