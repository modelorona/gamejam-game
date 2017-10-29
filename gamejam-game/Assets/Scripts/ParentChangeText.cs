using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentChangeText : MonoBehaviour {
    private TextMesh tm;
    private int startTime = 120;

	// Use this for initialization
	void Start ()       {
        tm = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if (startTime == 0)
        {
            tm.text = "Here lived Mr.\nand Mrs. B";
        }
        startTime--;
	}
}
