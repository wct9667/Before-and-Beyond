using UnityEngine;


namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputReader inputReader;
    
    
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
           // Debug.Log($"Look x:{lookVector.x}, y:{lookVector.y} ");
        }
    
        private void Jump()
        {
            Debug.Log("Jump");
        }
    }
}
