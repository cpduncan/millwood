using System;
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
    [SerializeField] private VisualTreeAsset m_controlTemplate;

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

        DisplayControls();
    }
    
    public void Unpause()
    {
        m_root.AddToClassList("hidden");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    private void DisplayControls()
    {
        VisualElement binds_content = m_root.Q<VisualElement>("binds_content");
        binds_content.Clear();

        InputActionMap actionMap = inputActionAsset.actionMaps[0];

        foreach (InputAction action in actionMap.actions)
        {
            m_controlTemplate.CloneTree(binds_content.contentContainer);
            VisualElement entry = binds_content.ElementAt(binds_content.contentContainer.childCount - 1);
            
            entry.Query<VisualElement>().ForEach(e =>
            {
                e.dataSource = action;
            });
        }
    }
}
