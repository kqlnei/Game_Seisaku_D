using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public GameObject checkObject; // BoxCollider������GameObject
    public GameObject triangleObject; // MeshRenderer�𐧌䂷��GameObject

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            // Player��CheckObject��BoxCollider�ɓ������Ƃ��ATriangleObject��MeshRenderer��L���ɂ���
            if (triangleObject != null && checkObject != null)
            {
              
                BoxCollider boxCollider = checkObject.GetComponent<BoxCollider>();
                if (boxCollider != null && boxCollider.bounds.Contains(transform.position))
                {
                  
                    MeshRenderer meshRenderer = triangleObject.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                      
                        meshRenderer.enabled = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            // Player��BoxCollider����o���Ƃ��ATriangleObject��MeshRenderer�𖳌��ɂ���
            if (triangleObject != null)
            {
             
                MeshRenderer meshRenderer = triangleObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                  
                    meshRenderer.enabled = false;
                }
            }
        }
    }
}
