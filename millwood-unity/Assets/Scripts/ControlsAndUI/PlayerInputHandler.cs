using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";
    [SerializeField] private string ui_actionMapName = "UI";

    [Header("Gameplay Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string shoot = "Shoot";
    [SerializeField] private string shootStop = "ShootStop";
    [SerializeField] private string decompose = "Decompose";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string reload = "Reload";
    [SerializeField] private string console = "Console";
    [SerializeField] private string escape = "Escape";
    
    [Header("UI Action Name References")]
    [SerializeField] private string ui_escape = "UI_Escape";

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction interactAction;
    private InputAction shootAction;
    private InputAction shootStopAction;
    private InputAction decomposeAction;
    private InputAction jumpAction;
    private InputAction reloadAction;
    private InputAction consoleAction;
    private InputAction escapeAction;
    
    private InputAction ui_escapeAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    
    public event Action Interact;
    public event Action Shoot;
    public event Action ShootStop;
    public event Action Decompose;
    public event Action Jump;
    public event Action Reload;
    public event Action Console;
    public event Action Escape;
    
    public event Action UI_Escape;

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);
        InputActionMap ui_mapReference = playerControls.FindActionMap(ui_actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        interactAction = mapReference.FindAction(interact);
        shootAction = mapReference.FindAction(shoot);
        shootStopAction = mapReference.FindAction(shootStop);
        decomposeAction = mapReference.FindAction(decompose);
        jumpAction = mapReference.FindAction(jump);
        reloadAction = mapReference.FindAction(reload);
        consoleAction = mapReference.FindAction(console);
        escapeAction = mapReference.FindAction(escape);
        
        ui_escapeAction = ui_mapReference.FindAction(ui_escape);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        interactAction.performed += _ => Interact?.Invoke();
        shootAction.performed += _ => Shoot?.Invoke();
        shootStopAction.performed += _ => ShootStop?.Invoke();
        decomposeAction.performed += _ => Decompose?.Invoke();
        jumpAction.performed += _ => Jump?.Invoke();
        reloadAction.performed += _ => Reload?.Invoke();
        consoleAction.performed += _ => Console?.Invoke();
        escapeAction.performed += _ => Escape?.Invoke();
        
        ui_escapeAction.performed += _ => UI_Escape?.Invoke();
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }

}
