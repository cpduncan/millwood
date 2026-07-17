using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PauseMenuManager : MonoBehaviour
{
    private UIDocument m_uiDocument;
    private VisualElement m_root;
    private VisualElement m_rebinding_overlay;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperationKeyboard;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperationGamepad;

    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private VisualTreeAsset asset_template_bindable;
    [SerializeField] private VisualTreeAsset asset_template_unbindable;
    [SerializeField] private VisualTreeAsset asset_template_spacer;

    private Dictionary<string, InputAction> actions = new Dictionary<string, InputAction>();

    private void Awake()
    {
        LoadOverrides();
        m_uiDocument = gameObject.GetComponent<UIDocument>();
        m_root = m_uiDocument.rootVisualElement.Q<VisualElement>("panel");
        m_root.AddToClassList("hidden");
        m_rebinding_overlay = m_uiDocument.rootVisualElement.Q<VisualElement>("rebinding_overlay");
    }

    public void Pause()
    {
        m_root.RemoveFromClassList("hidden");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Controls();
    }
    
    public void Unpause()
    {
        m_root.AddToClassList("hidden");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    private void Controls()
    {
        VisualElement binds_content = m_root.Q<VisualElement>("binds_content");
        binds_content.Clear();
        
        InputActionMap actionMap = inputActionAsset.actionMaps[0];
            
        DisplayUnbindableEntry(actionMap.FindAction("Movement"), binds_content);
        DisplayUnbindableEntry(actionMap.FindAction("Rotation"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("Jump"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("CrouchSlide"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("Sprint"), binds_content);
        DisplaySpacer(binds_content);
        DisplayBindableEntry(actionMap.FindAction("Vision"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("Interact"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("Melee"), binds_content);
        DisplaySpacer(binds_content);
        DisplayUnbindableEntry(actionMap.FindAction("Shoot"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("Reload"), binds_content);
        // just using interact for this DisplayBindableEntry(actionMap.FindAction("CollectBiomass"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("BioweaponPreset1"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("BioweaponPreset2"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("BioweaponPreset3"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("DecomposeBioweapon"), binds_content);
        DisplaySpacer(binds_content);
        DisplayBindableEntry(actionMap.FindAction("Console"), binds_content);
        // just switching to other input instead DisplayBindableEntry(actionMap.FindAction("TextOptionUp"), binds_content);
        // just switching to other input instead DisplayBindableEntry(actionMap.FindAction("TextOptionDown"), binds_content);
        // just switching to other input instead DisplayBindableEntry(actionMap.FindAction("TextOptionBack"), binds_content);
        // just switching to other input instead DisplayBindableEntry(actionMap.FindAction("TextOptionConfirm"), binds_content);
        DisplayBindableEntry(actionMap.FindAction("Menu"), binds_content);
        
    }

    private void DisplayBindableEntry(InputAction action, VisualElement binds_content)
    {
        asset_template_bindable.CloneTree(binds_content.contentContainer);
        VisualElement entry = binds_content.ElementAt(binds_content.contentContainer.childCount - 1);
        
        entry.Query<VisualElement>().ForEach(e =>
        {
            e.dataSource = action;
        });

        Button kbmRemapButton = entry.Q<Button>("kbm_remap_button");
        kbmRemapButton.clicked += () => { KbmRemapButton(action); };

        Button gamepadRemapButton = entry.Q<Button>("gamepad_remap_button");
        gamepadRemapButton.clicked += () => { GamepadRemapButton(action); };

        Button resetBindingButton = entry.Q<Button>("reset_binding_button");
        resetBindingButton.clicked += () => { ResetBinding(action); };
    }

    private void ResetBinding(InputAction action)
    {
        action.RemoveAllBindingOverrides();
        SaveBindings();
        Controls();
    }

    private void KbmRemapButton(InputAction action)
    {
        m_rebinding_overlay.RemoveFromClassList("hidden");
        
        action.Disable();
        rebindingOperationKeyboard = action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithTargetBinding(0)
            .OnMatchWaitForAnother(0.1f);
        rebindingOperationKeyboard.Start();
        rebindingOperationKeyboard.OnComplete(OnKbmRebindOperationComplete);
    }
    private void OnKbmRebindOperationComplete(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
    {
        m_rebinding_overlay.AddToClassList("hidden");
        Controls();
        rebindingOperation.action.Enable();
        rebindingOperationKeyboard.Cancel();
        SaveBindings();
    }

    private void GamepadRemapButton(InputAction action)
    {
        m_rebinding_overlay.RemoveFromClassList("hidden");
        
        action.Disable();
        rebindingOperationGamepad = action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithTargetBinding(1)
            .OnMatchWaitForAnother(0.1f);
        rebindingOperationGamepad.Start();
        rebindingOperationGamepad.OnComplete(OnGamepadRebindOperationComplete);
    }
    private void OnGamepadRebindOperationComplete(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
    {
        m_rebinding_overlay.AddToClassList("hidden");
        Controls();
        rebindingOperation.action.Enable();
        rebindingOperationGamepad.Cancel();
        SaveBindings();
    }

    private void DisplayUnbindableEntry(InputAction action, VisualElement bindings_container)
    {
        asset_template_unbindable.CloneTree(bindings_container.contentContainer);
        
        VisualElement entry = bindings_container.ElementAt(bindings_container.contentContainer.childCount - 1);
        
        entry.Query<VisualElement>().ForEach(e =>
        {
            e.dataSource = action;
        });
    }

    private void DisplaySpacer(VisualElement bindings_container)
    {
        asset_template_spacer.CloneTree(bindings_container.contentContainer);
    }

    private void LoadOverrides()
    {
        string path = string.Concat(Application.persistentDataPath, "/Persist/bindings.json");
        if (File.Exists(path))
        {
            string overrides = File.ReadAllText(path);
            inputActionAsset.LoadBindingOverridesFromJson(overrides);
        }
    }

    private void SaveBindings()
    {
        string overrides = inputActionAsset.SaveBindingOverridesAsJson();

        string directory = Path.Combine(Application.persistentDataPath, "Persist");
        Directory.CreateDirectory(directory);

        string filePath = Path.Combine(directory, "bindings.json");
        File.WriteAllText(filePath, overrides);
    }
    
}
