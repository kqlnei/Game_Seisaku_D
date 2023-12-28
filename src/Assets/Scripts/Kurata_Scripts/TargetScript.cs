using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public GameObject checkObject; // BoxColliderを持つGameObject
    public GameObject triangleObject; // MeshRendererを制御するGameObject

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            // PlayerがCheckObjectのBoxColliderに入ったとき、TriangleObjectのMeshRendererを有効にする
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
           
            // PlayerがBoxColliderから出たとき、TriangleObjectのMeshRendererを無効にする
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
