using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMovement : MonoBehaviour {

    GameObject gameObject;

    Animator anim;

    public float span = 5.0f;
    Vector3 position;
    bool add = true;
    float iterator = 1;
    Vector3 moveTo;
    // Use this for initialization
    Rigidbody rigidBody;
    float lastPosition;

    void Start()
    {
        //transform.Translate(new Vector3(1, -1, 0) * Time.deltaTime);
        position.x = transform.position.x;
        position.y = transform.position.y;
        position.z = transform.position.z;
        
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        gameObject = GameObject.FindGameObjectWithTag("MainCharacter");

        lastPosition = Vector3.Distance(gameObject.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveTo * Time.deltaTime);

        //if (Mathf.Abs(transform.position.x - position.x) > 5.0f && Vector3.Distance(gameObject.transform.position, transform.position) < 10f)
        //{
        //    rigidBody.position = position;
        //    rigidBody.AddForce(new Vector3(1, 0, 0) * -2.5f, ForceMode.Impulse);
        //}

        
        if (Vector3.Distance(gameObject.transform.position, transform.position) < 10f)
        {
            if (Vector3.Distance(gameObject.transform.position, transform.position) > lastPosition)
            {
                rigidBody.Sleep();
                anim.SetBool("IsRunning", false);
            }
            Vector3 direction = gameObject.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            
            if (direction.magnitude > 5)
            {
                transform.Translate(0, 0, 0.1f);
                anim.SetBool("IsRunning", true);
            }
            else
            {
                anim.SetTrigger("Attack");
                rigidBody.AddForce(direction, ForceMode.Impulse);
            }
            lastPosition = Vector3.Distance(gameObject.transform.position, transform.position);
        }
    }



    IEnumerator Pause(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
