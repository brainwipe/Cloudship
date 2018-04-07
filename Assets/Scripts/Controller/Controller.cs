using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    Animator animator;
    Cloudship player;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            if (!animator.GetBool("BuildMode"))
            {
                animator.SetBool("BuildMode", true);
            }
            else
            {
                animator.SetBool("BuildMode", false);
            }
            
        }
    }

}

