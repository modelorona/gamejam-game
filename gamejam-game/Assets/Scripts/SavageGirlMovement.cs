using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SavageGirlMovement : MonoBehaviour {


    public GameObject bulletPrefab;
    public Transform bulletSpawn;


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


    CharacterController characterController;
    Animator animator;


    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);
        Move(inputDir, running);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.RightControl))
            Shoot();

        if (Input.GetKeyUp(KeyCode.RightControl))
            StopShooting();

        CheckFall();

        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothVelocity, Time.deltaTime);

        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("level3").transform.position) <= 5)
            SceneManager.LoadScene("cave");


    }

    void Shoot()
    {
        for (int i = 0; i < 5; i++)
        {
            animator.SetBool("IsShooting", true);
            var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

            bullet.GetComponent<Rigidbody>().velocity = new Vector3(1.0f, 0, 0) * 15f;

            // Add velocity to the bullet


            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }
    }

    void StopShooting()
    {
        animator.SetBool("IsShooting", false);
    }

    void CheckFall()
    {
        if (!characterController.isGrounded && characterController.velocity.y < 0)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
    }

    void Jump()
    {
        if (characterController.isGrounded || (characterController.velocity.y == 0))
        {
            animator.SetBool("IsJumping", true);
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
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


}
