using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PauseMenuManager : MonoBehaviour
{
    private UIDocument m_uiDocument;
    private  VisualElement m_root;

    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private VisualTreeAsset asset_template_bindable;
    [SerializeField] private VisualTreeAsset asset_template_unbindable;
    [SerializeField] private VisualTreeAsset asset_template_spacer;

    private Dictionary<string, InputAction> actions = new Dictionary<string, InputAction>();

    private void Awake()
    {
        m_uiDocument = gameObject.GetComponent<UIDocument>();
        m_root = m_uiDocument.rootVisualElement.Q<VisualElement>("panel");
        m_root.AddToClassList("hidden");
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
        VisualElement bindings_container = m_root.Q<VisualElement>("binds_content");
        bindings_container.Clear();
        
        InputActionMap actionMap = inputActionAsset.actionMaps[0];
            
        DisplayUnbindableEntry(actionMap.FindAction("Movement"), bindings_container);
        // keep commented DisplayUnbindableEntry(actionMap.FindAction("Rotation"), bindings_container);
        DisplayBindableEntry(actionMap.FindAction("Jump"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("Crouch/Slide"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("Sprint"), bindings_container);
        DisplaySpacer(bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("Vision"), bindings_container);
        DisplayBindableEntry(actionMap.FindAction("Interact"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("Melee"), bindings_container);
        DisplaySpacer(bindings_container);
        DisplayBindableEntry(actionMap.FindAction("Shoot"), bindings_container);
        DisplayBindableEntry(actionMap.FindAction("Reload"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("CollectBiomass"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("GrowBioweapon1"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("GrowBioweapon2"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("GrowBioweapon3"), bindings_container);
        DisplayBindableEntry(actionMap.FindAction("DecomposeBioweapon"), bindings_container);
        DisplaySpacer(bindings_container);
        DisplayBindableEntry(actionMap.FindAction("Console"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("TextOptionUp"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("TextOptionDown"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("TextOptionBack"), bindings_container);
        // DisplayBindableEntry(actionMap.FindAction("TextOptionConfirm"), bindings_container);
        DisplayBindableEntry(actionMap.FindAction("Menu"), bindings_container);
        
    }

    private void DisplayBindableEntry(InputAction action, VisualElement bindings_container)
    {
        asset_template_bindable.CloneTree(bindings_container.contentContainer);
        
        VisualElement entry = bindings_container.ElementAt(bindings_container.contentContainer.childCount - 1);
        
        entry.Query<VisualElement>().ForEach(e =>
        {
            e.dataSource = action;
        });
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
    
}
