using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public enum Device
{
    Gamepad,
    Keyboard
}
[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, InputSystem_Actions.IUIActions, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    
    private InputDevice device;
    public InputDevice DeviceType => device;
    
    /// <summary>
    /// UI Actions
    /// </summary>
    public event UnityAction<Vector2> Navigate = delegate { };
    public event UnityAction<Vector2> Point = delegate { };
    public event UnityAction<Vector2> ScrollWheel = delegate { };
    public event UnityAction Submit = delegate { };
    public event UnityAction Cancel = delegate { };
    public event UnityAction Click = delegate { };
    public event UnityAction RightClick = delegate { };
    public event UnityAction MiddleClick = delegate { };

    //Gameplay Action
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction<Vector2, bool> Look = delegate { };
    public event UnityAction Interact = delegate { };
    public event UnityAction Jump = delegate { };
    public event UnityAction SwapCharacter = delegate { };
    public event UnityAction StartingAbility = delegate { };
    
    public event UnityAction SecondAbility = delegate { };
    public event UnityAction ThirdAbility = delegate { };

    public event UnityAction Pause = delegate { };

    /// <summary>
    /// Create InputAction class if not exist and set the callbacks
    /// </summary>
    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();
            inputActions.UI.SetCallbacks(this);
            inputActions.Player.SetCallbacks(this);
        }
        inputActions.Player.Enable();
        inputActions.UI.Enable();
    }

    private void OnDisable()
    { 
        inputActions.UI.Disable();
        inputActions.Player.Disable();
    }

    /// <summary>
    /// Sets Input Mode to UI
    /// </summary>
    public void SetUIInputMap()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        inputActions.UI.Enable();
        inputActions.Player.Disable();
    }
    
    /// <summary>
    /// Sets the input mode to Game
    /// </summary>
    public void SetGameInputMap()
    {
        Cursor.visible = false;
        inputActions.UI.Disable();
        inputActions.Player.Enable();
    }
    

    #region GameInputEvents

    public void OnMove(InputAction.CallbackContext context)
    {
        device = context.control.device;
        Move.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        device = context.control.device;
        Look.Invoke(context.ReadValue<Vector2>(), context.control.device is Gamepad);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        device = context.control.device;
        if(context.phase == InputActionPhase.Started)
            Interact.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        device = context.control.device;
        if(context.phase == InputActionPhase.Performed)
            Jump.Invoke();
    }

    public void OnSwapCharacter(InputAction.CallbackContext context)
    {
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed)
            SwapCharacter.Invoke();
    }

    public void OnStartingAbility(InputAction.CallbackContext context)
    {
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed) 
            StartingAbility.Invoke();
    }
    
    public void OnSecondAbility(InputAction.CallbackContext context)
    {
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed) 
            SecondAbility.Invoke();
    }

    public void OnThirdAbility(InputAction.CallbackContext context)
    {
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed)
            ThirdAbility.Invoke();
    }

    #endregion

    #region UIInputEvents
    public void OnNavigate(InputAction.CallbackContext context)
    {
        device = context.control.device;
        Navigate.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        device = context.control.device;
        Point.Invoke(context.ReadValue<Vector2>());
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    { 
        device = context.control.device;
        ScrollWheel.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSubmit(InputAction.CallbackContext context)
    { 
        device = context.control.device;
        Submit.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    { 
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed)
            Cancel.Invoke();
    }

    public void OnClick(InputAction.CallbackContext context)
    { 
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed)
            Click.Invoke();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    { 
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed)
            RightClick.Invoke();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    { 
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed)
            MiddleClick.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        device = context.control.device;
        if (context.phase == InputActionPhase.Performed)
            Pause.Invoke();
    }

    //These were UI events, wont be used but have to be implemented
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        device = context.control.device;
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        device = context.control.device;
    }
    
    #endregion
}