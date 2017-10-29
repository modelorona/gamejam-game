using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFalling : MonoBehaviour {

    Rigidbody rigidbody;
    Transform trans;

    public GameObject rockPrefab;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        trans = transform;
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");   
        if(collision.gameObject.tag == "MainCharacter")
        {
            Wait(0.5f);
            rigidbody.AddForce(Vector3.down*2.5f, ForceMode.Impulse);
            Destroy(rigidbody,2.0f);
            Wait(0.5f);
            Instantiate(rockPrefab, trans);
        }
    }


    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
