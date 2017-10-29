using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AxeGirlRespawn : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float speedSmoothTime = 0.05f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    public GameObject mainCharacterPrefab;

    CharacterController characterController;


    GameObject mainCamera;
    private MainCharacterFollow mainCharacterFollow;


    // Use this for initialization
    void Start () {

        characterController = GetComponent<CharacterController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCharacterFollow = mainCamera.GetComponent<MainCharacterFollow>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);
        Move(inputDir, running);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyUp(KeyCode.RightControl))
            StopAttack();

        GameObject gO = FindClosestEnemy();
        if(Vector3.Distance(gO.transform.position,transform.position)<10)
        {
            Destroy(gO);
        }

        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("level3").transform.position) <= 5)
            SceneManager.LoadScene("cave");


    }

    public void StopAttack()
    {
        Transform lastPosition = transform;
        Destroy(GameObject.FindGameObjectWithTag("axeGirl"));
        Instantiate(mainCharacterPrefab, (lastPosition.position + lastPosition.forward * 2.5f), transform.rotation * Quaternion.Euler(0, 180, 0));
        mainCharacterFollow.rebootCamera("MainCharacter");
    }


    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        characterController.Move(velocity * Time.deltaTime);
        velocityY += Time.deltaTime * gravity;
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
        if (characterController.isGrounded)
            velocityY = 0;


        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
    }

    void Jump()
    {
        if (characterController.isGrounded || (characterController.velocity.y == 0))
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (characterController.isGrounded)
        {
            return smoothTime;
        }
        if (airControlPercent == 0)
            return float.MaxValue;
        return smoothTime / airControlPercent;
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
