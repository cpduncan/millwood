using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Console : MonoBehaviour
{
    
    [SerializeField] private int console_history_height;
    
    private string[] console_history;
    // private ConsoleLine[] console_lines; instantiate console line prefab onto the UI canvas
    private string console_input = "";

    public bool console_focus = true;
    
    public TMP_InputField inputField;
    
    void Start()
    {
        console_history = new string[console_history_height];
        // console_lines = new ConsoleLine[console_history_height];
    }

    void FocusConsole()
    {
    }

    void PrintConsole()
    {
        foreach (string line in console_history)
        {
        }
    }
}
