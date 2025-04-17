using System.Collections;
using UnityEngine;
using UnityEngine.UI;


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
        private bool isGamepad = false;
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


        [Header("Jump")] 
        [SerializeField] private float groundDistance;
        private int jumpCount;

        [Header("Health")]
        [SerializeField] private Slider knightHealthBar;
        [SerializeField] private Slider sciFiHealthBar;
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float currentHealth;
        [SerializeField] private Image knightHealthBarFill;
        [SerializeField] private Image sciFiHealthBarFill;

        private void Start()
        {
            playerState = GetComponent<PlayerState>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            currentHealth = maxHealth;
            knightHealthBar.maxValue = maxHealth;
            sciFiHealthBar.maxValue = maxHealth;
            knightHealthBar.value = currentHealth;
            sciFiHealthBar.value = currentHealth;
        }

        private void Update()
        {
            knightHealthBar.value = currentHealth;
            sciFiHealthBar.value = currentHealth;
            if (currentHealth <= 30)
            {
                knightHealthBarFill.color = Color.red;
                sciFiHealthBarFill.color = Color.red;
            }
        }
        private void FixedUpdate()
        {
            Vector2 moveVector = new Vector2(horizontalInput, verticalInput);
            Move(moveVector);
            
            if(isGamepad) Look(currentLookInput);

            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + groundDistance, whatIsGround);
            if (grounded) jumpCount = 0;
            


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

        private void OnLook(Vector2 lookVector, bool isGamepad)
        {
            currentLookInput = lookVector;
            this.isGamepad = isGamepad;
            if (!isGamepad) Look(currentLookInput);
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
        
        /// <summary>
        /// Jumps up, cancels current y
        /// Only jumps if double jump available or if grounded
        /// </summary>
        private void Jump()
        {
            if (!grounded && !playerState.CurrentCharacter.canDoubleJump) return;
            
            switch (jumpCount)
            {
                case 0: 
                    break;
                default: 
                    return;
            }

            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(transform.up * playerState.CurrentCharacter.jumpForce, ForceMode.Impulse);
            jumpCount++;
        }



        public void Grapple(float startTime, RaycastHit hit)
        {
            StartCoroutine(GrappleMovement(startTime, hit));
        }
        private IEnumerator GrappleMovement(float startTime, RaycastHit hit)
        {
            while (Vector3.Distance(transform.position, hit.point) > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, hit.point, (Time.time - startTime));

                rb.AddForce(0, 0.2f, 0, ForceMode.Impulse);

                yield return null;

            }
            yield return new WaitForSeconds(2f);
        }
    }
}
