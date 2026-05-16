using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    [SerializeField] string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        
        SceneManager.LoadScene(sceneName);
        
    }
    
}
