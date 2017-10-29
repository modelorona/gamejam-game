using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCharacterFollow : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;

    TextMesh textMesh;

    // Use this for initialization
    void Start () {
        offset = transform.position - player.transform.position;

        textMesh = GetComponent<TextMesh>();
        textMesh.text = "";

        player = GameObject.FindGameObjectWithTag("MainCharacter");
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position + offset;
    }

    public void rebootCamera(string stri)
    {
        player = GameObject.FindWithTag(stri);
    }

    public void set(GameObject player)
    {
        this.player = player;
    }

    public void setTransformPosition(Vector3 vec)
    {
        transform.position = vec;
    }

    public void gameOver()
    {
        textMesh.text = "GameOver!";
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        
    }
}
