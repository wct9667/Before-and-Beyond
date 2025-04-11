using UnityEngine;


namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputReader inputReader;

        public Camera mainCamera;
        public float sensX;
        public float sensY;

        public Transform orientation;

        float rotationX;
        float rotationY;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
            Debug.Log($"Move x:{moveVector.x}, y:{moveVector.y} ");
        }
        
        private void Look(Vector2 lookVector)
        {
            float mouseX = lookVector.x * sensX * Time.deltaTime;
            float mouseY = lookVector.y * sensY * Time.deltaTime;

            rotationY += mouseX;
            rotationX -= mouseY;

            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            mainCamera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            orientation.rotation = Quaternion.Euler(0, rotationY, 0);
            Debug.Log($"Look x:{lookVector.x}, y:{lookVector.y} ");
        }
    
        private void Jump()
        {
            Debug.Log("Jump");
        }
    }
}
