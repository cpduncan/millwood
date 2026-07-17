using System;
using NUnit.Framework.Internal;
using QuakeLR;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class FirstPersonController : MonoBehaviour
{
    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField] private float upDownLookRange = 90f;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private PlayerDataController playerData;
    [SerializeField] private Console console;
    [SerializeField] private WeaponSlot weaponSlot;

    private QuakeCharacterController _quakeCharacterController = null;
    
    private float _verticalRotation;
    
    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private PauseMenuManager pauseMenuManager;
    

    private void OnEnable()
    {
        playerInputHandler.Interact += EventInteract;
        playerInputHandler.Shoot += EventShoot;
        playerInputHandler.ShootStop += EventShootStop;
        playerInputHandler.DecomposeBioweapon += EventDecomposeBioweapon;
        playerInputHandler.Jump += EventJump;
        playerInputHandler.Reload += EventReload;
        playerInputHandler.Console += EventConsole;
        playerInputHandler.Menu += EventMenu;
        
        playerInputHandler.UI_Menu += UI_EventMenu;
    }
    
    void Awake()
    {
        _quakeCharacterController = GetComponent<QuakeCharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void EventConsole()
    {
        
    }

    // #####################################################################
    //region pause UI management
    
    private void EventMenu()
    {
        Time.timeScale = 0f;
        inputActionAsset.FindActionMap("Player").Disable();
        inputActionAsset.FindActionMap("UI").Enable();

        pauseMenuManager.Pause();
    }
    
    private void UI_EventMenu()
    {
        Time.timeScale = 1f;
        inputActionAsset.FindActionMap("UI").Disable();
        inputActionAsset.FindActionMap("Player").Enable();
        
        pauseMenuManager.Unpause();
    }
    
    //endregion
    // #####################################################################

    private void EventInteract()
    {
        Debug.Log("Interact");
    }

    private void EventShoot()
    {
        weaponSlot.Shoot();
    }

    private void EventShootStop()
    {
        weaponSlot.ShootStop();
    }
    
    private void EventDecomposeBioweapon()
    {
        playerData.ChangeAmmo(weaponSlot.Mill());
    }

    private void EventReload()
    {
        weaponSlot.Reload();
    }
    
    private void EventJump()
    {
        _quakeCharacterController.TryJump();
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
