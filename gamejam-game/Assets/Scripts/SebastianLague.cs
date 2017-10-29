 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SebastianLague : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    public float jumpHeight = 1;
    [Range(0,1)]
    public float airControlPercent;

    public float speedSmoothTime = 0.05f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    public GameObject savagePrefab;

    CharacterController characterController;
    Animator animator;

    bool dead;
    bool deadOnce;

    Transform lastPosition;
    Transform lastPositionCamera;

    public GameObject mainCamera;
    private MainCharacterFollow mainCharacterFollow;

    public GameObject axeGirlPrefab;
    public GameObject cameraPrefab;
    public GameObject mainCharacterPrefab;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        dead = false;
        deadOnce = false;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCharacterFollow = mainCamera.GetComponent<MainCharacterFollow>();

    }
	
	// Update is called once per frame
	void Update () {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        Move(inputDir, running);

        GameObject gO = FindClosestEnemy();

        
        if (Vector3.Distance(transform.position, gO.transform.position) >= 15)
        {
            lastPosition = transform;
            lastPositionCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            Debug.Log(lastPosition);
        }

        if (Vector3.Distance(transform.position, gO.transform.position)<=4 && !deadOnce)
        {
            Debug.Log("DeadOnce");
            deadOnce = true;
            //Destroy(GameObject.FindGameObjectWithTag("MainCharacter"));
            //savagePrefab.tag = "MainCharacter";
            //Instantiate(savagePrefab, (lastPosition.position + lastPosition.forward * 2.5f), transform.rotation * Quaternion.Euler(0, 90, 0));
            //Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
            //Instantiate(cameraPrefab, (lastPosition.position + lastPosition.right * -20f), transform.rotation * Quaternion.Euler(0, -270, 0));
            //Debug.Log("Instantiate");
        }
        if(deadOnce &&!dead)
        {
            for(int i=0;i<15;i++)
                transform.Translate(new Vector3(0, 1, 0));
            dead = true;
        }

        if(dead)
        {
            lastPosition = transform;
            Destroy(GameObject.FindGameObjectWithTag("MainCharacter"));
            Instantiate(savagePrefab, (lastPosition.position + lastPosition.forward * 2.5f), transform.rotation * Quaternion.Euler(0, 180, 0));
            //Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
            //Instantiate(cameraPrefab, (lastPosition.position + lastPosition.right * -40f), transform.rotation * Quaternion.Euler(0, -270, 0));
            //Debug.Log("Instantiate");
            mainCharacterFollow.rebootCamera("savage");



        }

       // if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("level3").transform.position) <= 5)
         //   SceneManager.LoadScene("cave");



        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.RightControl))
            Attack();
        

        CheckFall();

        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothVelocity, Time.deltaTime);
    }

    void CheckFall()
    {
        if(!characterController.isGrounded && characterController.velocity.y<0)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
    }

    public void Attack()
    {
        lastPosition = transform;
        Destroy(GameObject.FindGameObjectWithTag("MainCharacter"));
        Instantiate(axeGirlPrefab, (lastPosition.position + lastPosition.forward), transform.rotation);
        mainCharacterFollow.rebootCamera("axeGirl");
    }

    public void StopAttack()
    {
        lastPosition = transform;
        Destroy(GameObject.FindGameObjectWithTag("MainCharacter"));
        Instantiate(mainCharacterPrefab, (lastPosition.position + lastPosition.forward * 2.5f), transform.rotation * Quaternion.Euler(0, 180, 0));
        mainCharacterFollow.rebootCamera("MainCharacter");
    }

    void Jump()
    {
        if(characterController.isGrounded || (characterController.velocity.y==0))
        {
            animator.SetBool("IsJumping", true);
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }


    void Move(Vector2 inputDir,bool running)
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

    float GetModifiedSmoothTime(float smoothTime)
    {
        if(characterController.isGrounded)
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
