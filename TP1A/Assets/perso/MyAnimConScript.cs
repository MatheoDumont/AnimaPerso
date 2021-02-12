using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimConScript : MonoBehaviour
{
    public Animator myAnimator;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        // Debug.Log("MyAniConScript: start => Animator");
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        myAnimator.SetFloat("HSpeed", -Input.GetAxis("Horizontal"));
        // myAnimator.SetBool("Jumping", Input.GetAxis("Jump") > 0);
        myAnimator.SetBool("IsJumping", Input.GetKey(KeyCode.Space));
        // Debug.Log(Input.GetKey(KeyCode.Space));
        // myAnimator.SetFloat("Jump", Input.GetAxis("Jump"));
    }
}
