using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class nitrocontrol : MonoBehaviour {
    [SerializeField]
    public bool hit { get; set; }



    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (CrossPlatformInputManager.GetButtonDown("Nitrous") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
        {
            anim.SetTrigger("hit");

        }



    }
}
