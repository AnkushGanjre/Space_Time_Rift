using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintSpeed = 7f;
    [SerializeField] float rotationSpeed = 350f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float climbHeight = 3.5f;
    [SerializeField] float animDampTime = 0.15f;

    private Vector2 playerMoveInput;
    private Vector3 velocity;

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;
    bool isSprinting;
    bool isJumping;
    bool isCrouching;
    bool isClimbing;

    float ySpeed;
    Quaternion targetRotation;

    CameraController cameraController;
    Animator anim;
    CharacterController characterController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var moveInput = (new Vector3(playerMoveInput.x, 0, playerMoveInput.y)).normalized;
        float moveAmount = Mathf.Clamp(Mathf.Abs(playerMoveInput.x) + Mathf.Abs(playerMoveInput.y), 0.0f, 0.2f);
        var moveDir = cameraController.PlanerRotation * moveInput;

        // Applying Gravity
        GroundCheck();
        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else if (!isJumping)
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        // If Crouching
        if (isCrouching)
        {
            velocity = moveDir * walkSpeed/1.5f;
            characterController.Move(velocity * Time.deltaTime);
            if (moveAmount > 0)
            {
                targetRotation = Quaternion.LookRotation(moveDir);
                moveAmount = 0 - moveAmount;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            moveAmount -= 0.3f;
            anim.SetFloat("moveAmount", moveAmount, 0.1f, Time.deltaTime);
            return;
        }

        // Check Walk Or Run
        if (isSprinting)
        {
            velocity = moveDir * sprintSpeed;
            if (moveAmount > 0)
            {
                // To get from 0.2 to 1 in 1 Second for smooth moveAmount Transition
                moveAmount = Mathf.Lerp(moveAmount, 1, 1);
            }
        }
        else
        {
            velocity = moveDir * walkSpeed;
        }
        
        // Applying Jump
        if (isJumping && isGrounded && !isClimbing)
        {
            //isGrounded = false;
            ySpeed = Mathf.Lerp(ySpeed, jumpHeight, 1);
            StartCoroutine(JumpWaitTime());
        }
        
        // Applying Climb
        if (isClimbing && isGrounded && !isJumping)
        {
            //isGrounded = false;
            ySpeed = Mathf.Lerp(ySpeed, climbHeight, 1);
            StartCoroutine(ClimbWaitTime());
        }

        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        anim.SetFloat("moveAmount", moveAmount, animDampTime, Time.deltaTime);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMoveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSprinting = true;
        }
        if (context.canceled)
        {
            isSprinting = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && !isCrouching)
        {
            isJumping = true;
            anim.SetTrigger("Jumping");
        }
    }

    private IEnumerator JumpWaitTime()
    {
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isCrouching)
            {
                isCrouching = false;
            }
            else
            {
                isCrouching = true;
            }
        }
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isClimbing = true;
            anim.SetTrigger("Climbing");
        }
    }

    private IEnumerator ClimbWaitTime()
    {
        GetComponent<CharacterController>().radius = 0.4f;
        GetComponent<CharacterController>().height = 0.4f;
        yield return new WaitForSeconds(0.4f);
        GetComponent<CharacterController>().radius = 0.5f;
        GetComponent<CharacterController>().height = 1.8f;
        isClimbing = false;
    }
}
