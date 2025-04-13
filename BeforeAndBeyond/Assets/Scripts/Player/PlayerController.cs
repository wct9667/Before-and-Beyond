using UnityEngine;


namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputReader inputReader;

        [Header("Settings")] 
        [SerializeField] private Settings settings;

        [Header("Look")]
        public Camera mainCamera;
        
        public Transform orientation;

        float rotationX;
        float rotationY;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        public bool grounded;

        [Header("Movement")]
        public float moveSpeed;
        float horizontalInput;
        float verticalInput;
        Vector3 moveDirection;
        Rigidbody rb;
        public float groundDrag;

        [Header("Jump")]
        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplier;
        public bool readyToJump;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            Vector2 moveVector = new Vector2(horizontalInput, verticalInput);
            Move(moveVector);

            //ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            

            if (grounded)
            {
                rb.drag = groundDrag;
                readyToJump = true;
            }
            else
            {
                rb.drag = 0;
                readyToJump = false;
            }
        }

        private void OnEnable()
        {
            //setup input maps
            inputReader.Move += Move;
            inputReader.Look += Look;
            inputReader.Jump += Jump;
        }
        
        
        private void OnDisable()
        {
            //setup input maps
            inputReader.Move -= Move;
            inputReader.Look -= Look;
            inputReader.Jump -= Jump;
        }
    
        private void Move(Vector2 moveVector)
        {
            horizontalInput = moveVector.x;
            verticalInput = moveVector.y;

            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * moveSpeed * 2f, ForceMode.Force);

            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * groundDrag, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
            }
            Debug.Log($"Move x:{moveVector.x}, y:{moveVector.y} ");
        }
        
        private void Look(Vector2 lookVector)
        {
            float mouseX = lookVector.x * settings.sensX * Time.deltaTime;
            float mouseY = lookVector.y * settings.sensY * Time.deltaTime;

            rotationY += mouseX;
            rotationX -= mouseY;

            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            mainCamera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            orientation.rotation = Quaternion.Euler(0, rotationY, 0);

            // Debug.Log($"Look x:{lookVector.x}, y:{lookVector.y} ");
        }
    
        private void Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // freeze y velocity

            if (grounded && readyToJump)
            {
                readyToJump = false;
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            Debug.Log("Jump");
        }

        private void ResetJump()
        {
            readyToJump = true;
        }
    }
}
