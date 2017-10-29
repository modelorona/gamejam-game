using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScript : MonoBehaviour {

    public GameObject gameObject;
    public GameObject savageObject;
    public Camera mainCamera;
    private MainCharacterFollow mainCharacterFollow;
    float startTime;
    private int tryy;
    float i = 0;
    float distance = 25.0f;

    // Use this for initialization
    void Start () {
        tryy = 1;
        startTime = 0;
        mainCharacterFollow= mainCamera.GetComponent<MainCharacterFollow>();
        mainCharacterFollow.set(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<CharacterController>().velocity.y < 0 && !gameObject.GetComponent<CharacterController>().isGrounded && i!=-1)
            i += 1;

            if (gameObject.GetComponent<CharacterController>().isGrounded)
                i = 0;
            
            if(i>180)
            {
                i = -1;
                //Instantiate(savageObject, (transform.position + transform.forward * distance), transform.rotation* Quaternion.Euler(0, 90, 0));
                //mainCharacterFollow.player = savageObject;
                //gameObject = savageObject;
                mainCharacterFollow.gameOver();
                //Destroy(gameObject);
            }


            //if (Vector3.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("level2").transform.position) <= 1)
            //    SceneManager.LoadScene("forest");
        

        

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
    }



}
