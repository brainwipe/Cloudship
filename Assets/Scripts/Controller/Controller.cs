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
        if (Input.GetKeyUp(KeyCode.B))
        {
            if (!animator.GetBool("BuildMode"))
            {
                animator.SetBool("BuildMode", true);
                player.Mode = Cloudship.Modes.Build;
            }
            else
            {
                animator.SetBool("BuildMode", false);
                player.Mode = Cloudship.Modes.Drive;
            }
        }
    }

}
