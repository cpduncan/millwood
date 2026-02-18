using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string shoot = "Shoot";
    [SerializeField] private string mill = "Mill";

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction interactAction;
    private InputAction shootAction;
    private InputAction millAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    
    public event Action Interact;
    public event Action Shoot;
    public event Action Mill;

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        
        interactAction = mapReference.FindAction(interact);
        shootAction = mapReference.FindAction(shoot);
        millAction = mapReference.FindAction(mill);

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
        millAction.performed += _ => Mill?.Invoke();
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
