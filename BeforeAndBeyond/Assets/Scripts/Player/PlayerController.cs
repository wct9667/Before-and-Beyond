using System;
using Unity.VisualScripting;
using UnityEngine;


namespace Player
{
    [RequireComponent(typeof(PlayerState))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerState playerState;
        
        [Header("Input")]
        [SerializeField] private InputReader inputReader;

        [Header("Settings")] 
        [SerializeField] private Settings settings;

        [Header("Look")]
        [SerializeField] Camera mainCamera;
        [SerializeField] Transform orientation;
        private bool isController = false;
        private Vector2 currentLookInput;
        float rotationX;
        float rotationY;

        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private bool grounded;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxSpeed = 100;
        [SerializeField] private float airMovementScalar = 1f;
        
        float horizontalInput;
        float verticalInput;
        Vector3 moveDirection;
        Rigidbody rb;
        [SerializeField] private float groundDrag;

        [Header("Jump")]
        [SerializeField] private float jumpForce;
        public int jumpCount;
        
        private void Start()
        {
            playerState = GetComponent<PlayerState>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            Vector2 moveVector = new Vector2(horizontalInput, verticalInput);
            Move(moveVector);
            
            if(isController) Look(currentLookInput);

            //ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            

            if (grounded)
            {
                rb.drag = groundDrag;
                jumpCount = 0;
            }
            else
            {
                rb.drag = 0;
            }
            
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

        private void OnEnable()
        {
            inputReader.Move += OnMove;
            inputReader.Look += OnLook;
            inputReader.Jump += Jump;
        }
        
        
        private void OnDisable()
        {
            inputReader.Move -= OnMove;
            inputReader.Look -= OnLook;
            inputReader.Jump -= Jump;
        }


        private void OnMove(Vector2 moveVector)
        {
            horizontalInput = moveVector.x;
            verticalInput = moveVector.y;
        }
        private void Move(Vector2 moveVector)
        {
            horizontalInput = moveVector.x;
            verticalInput = moveVector.y;

            moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

            float percentIncrease = playerState.CurrentCharacter.percentSpeedIncrease;
            float speedMultiplier = 1f + (percentIncrease / 100f);

            float airDrag = grounded ? 1 : airMovementScalar;
            Vector3 targetVelocity = moveDirection * (moveSpeed * speedMultiplier * airDrag);
            
            Vector3 currentVelocity = rb.velocity;
            Vector3 velocityChange = targetVelocity - new Vector3(currentVelocity.x, 0, currentVelocity.z);
            
            rb.AddForce(new Vector3(velocityChange.x, 0, velocityChange.z), ForceMode.VelocityChange);
        }

        private void OnLook(Vector2 lookVector, bool isController)
        {
            
            currentLookInput = lookVector;
            this.isController = isController;
            if (!isController) Look(currentLookInput);
        }

        private void Look(Vector2 lookVector)
        {
            float mouseX = lookVector.x * settings.sensX * Time.deltaTime;
            float mouseY = lookVector.y * settings.sensY * Time.deltaTime;
           
            rotationY += mouseX;
            rotationX -= mouseY;

            rotationX = Mathf.Clamp(rotationX, -90f, 90f );
            
            mainCamera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            orientation.rotation = Quaternion.Euler(0, rotationY, 0);
        }
        
        private void Jump()
        {
            if (playerState.CurrentCharacter.canDoubleJump)
            {
                switch (jumpCount)
                {
                    case 0:
                    case 1:
                        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                        jumpCount++;
                        return;
                    default:
                        return;
                }
            }

            if (!grounded) return;
            
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
