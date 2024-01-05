using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupBoxvarious : MonoBehaviour
{
    private Animator animator;
    public float animStart;
    public float destroyDelay = 10f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("ActivateSurpTrigger", animStart);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ActivateSurpTrigger()
    {
        if (animator != null)
        {
            animator.SetTrigger("Surp");
            Invoke("DestroyGameObject", destroyDelay);
        }
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
