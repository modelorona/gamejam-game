using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLvl2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}

    void OnCollision(Collision col)
    {
        Debug.Log("collisions here");
        if (col.gameObject.name == "MainCharacter")
        {
            Debug.Log("log collides with main character");
        }
    }
}
