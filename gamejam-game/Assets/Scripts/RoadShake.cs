using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadShake : MonoBehaviour { // work in progress. shakes the road when player is on it to symbolize crappy road
    private bool move;
    private bool up;
	// Use this for initialization
	void Start () {
        move = false;
        up = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (move)
        {
            if (up) { 
                transform.Translate(0, 0.1f, 0);
                up = false;
            }
            else
            {
                transform.Translate(0, -0.1f, 0);
                up = true;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "girl")
        {
            move = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "girl")
        {
            move = false;
        }
    }
}
