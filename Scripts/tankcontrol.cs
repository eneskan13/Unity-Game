using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class tankcontrol : MonoBehaviour
{
    [SerializeField]
    public bool hit { get; set; }

    float dirX, moveSpeed;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
       	moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
       dirX = Input.GetAxisRaw ("Horizontal") * moveSpeed * Time.deltaTime;

      transform.position = new Vector2 (transform.position.x + dirX, transform.position.y);



        if (dirX != 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
        {
            //anim.SetBool ("isWalking", true);
        }
        else
        {
           //anim.SetBool ("isWalking", false);
        }

        if (CrossPlatformInputManager.GetButtonDown("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
        {
       //   anim.SetBool ("isWalking", false);
            anim.SetTrigger("hit");

        }



    }
}
