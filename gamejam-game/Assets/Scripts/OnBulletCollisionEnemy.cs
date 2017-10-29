using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBulletCollisionEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        GameObject gO = FindClosestEnemy();
        if (Vector3.Distance(gO.transform.position, transform.position) < 4 && Mathf.Abs(gO.transform.position.y-transform.position.y)<2)
            Destroy(gO);

	}


    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }


}
