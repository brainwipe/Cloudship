using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    Animator animator;
    Cloudship player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Instance.PlayerCloudship;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (animator.GetBool("CinematicCamera"))
            {
                animator.SetBool("CinematicCamera", false);
            }
            else
            {
                animator.SetBool("CinematicCamera", true);
            }

            if (animator.GetBool("BuildMode"))
            {
                animator.SetBool("BuildMode", false);
                player.SetBuildModeOff();
            }
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            if (animator.GetBool("BuildMode"))
            {
                animator.SetBool("BuildMode", false);
                player.SetBuildModeOff();
            }
            else
            {
                animator.SetBool("BuildMode", true);
                player.SetBuildModeOn();
            }
        }
    }

}

