using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    [SerializeField] string sceneName;
    [SerializeField] LoadSceneMode loadSceneMode;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }
    
}
