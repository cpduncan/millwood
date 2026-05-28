using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleEntryPoint : MonoBehaviour
{

    [SerializeField] private GameObject systemsPrefab;
    [SerializeField] private string sceneName;
    
    [SerializeField] private Vector3 playerPositionOffset;
    [SerializeField] private Quaternion playerRotationOffset;
    
    [SerializeField] private Vector3 sceneTransformOffset;
    [SerializeField] private Quaternion sceneRotationOffset;

    private void Awake()
    {
        GameObject systems = Instantiate(systemsPrefab, transform.position + playerPositionOffset, transform.rotation * playerRotationOffset);
        
        // so the single entry point scene isn't destroyed while initializing objects
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        yield return null; // waits 1 frame
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }  
    
}
