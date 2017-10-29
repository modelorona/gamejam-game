using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMovement : MonoBehaviour {
    // Use this for initialization
    static Animator anim;
    public float speed = 10.0f; // og is 2
    public float rotationSpeed = 75.0f;
    private Rigidbody rigidBody;
    private bool isGrounded;

    void Start () {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        isGrounded = true; // to prevent multiple jumping in air. can be changed if we allow for double jumping
    }

    // Update is called once per frame
    void Update () {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if(Input.GetKeyDown("space"))
        {
            if (isGrounded)
            {
                rigidBody.AddForce(new Vector3(0, 7.0f, 0), ForceMode.Impulse);
                isGrounded = false;
            }
        }
        if (translation != 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
