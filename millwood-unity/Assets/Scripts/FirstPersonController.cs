using System;
using QuakeLR;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField] private float upDownLookRange = 90f;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;

    private QuakeCharacterController _quakeCharacterController = null;
    
    private float _verticalRotation;

    private void OnEnable()
    {
        playerInputHandler.Interact += EventInteract;
        playerInputHandler.Shoot += EventShoot;
        playerInputHandler.Mill += EventMill;
        playerInputHandler.Jump += EventJump;
    }
    
    void Awake()
    {
        _quakeCharacterController = GetComponent<QuakeCharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void EventInteract()
    {
        Debug.Log("Interact");
    }

    private void EventShoot()
    {
        Debug.Log("Shoot");
    }
    
    private void EventMill()
    {
        Debug.Log("Mill");
    }

    private void EventJump()
    {
        _quakeCharacterController.TryJump();
        Debug.Log("Jump");
    }

    void Update()
    {
        // UPDATE MOVEMENT ###
        float inputRight   = playerInputHandler.MovementInput.x;
        float inputForward = playerInputHandler.MovementInput.y;
        
        _quakeCharacterController.Move((transform.forward * inputForward + transform.right * inputRight).normalized);
        _quakeCharacterController.ControllerThink(Time.deltaTime);
        // ###
        
        // UPDATE ROTATION ###
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;
    
        transform.Rotate(0, mouseXRotation, 0);
        
        _verticalRotation = Mathf.Clamp(_verticalRotation - mouseYRotation, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        // ###
    }


}
