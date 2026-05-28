using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private bool spawnOnStart;
    
    [SerializeField] private GameObject prefab;
    [SerializeField] private float heightOffset;
    private Vector3 positionOffset;
    [SerializeField] private Quaternion rotationOffset;
    
    [SerializeField] private Color gizmoColor;
    
    private void Update()
    {
        if (!spawnOnStart) return;
        Spawn();
    }

    public void Spawn()
    {
        Instantiate(prefab,  transform.position + positionOffset, transform.rotation * rotationOffset);
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        positionOffset = Vector3.up * heightOffset;
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position + (positionOffset * 0.5f), Vector3.one * heightOffset);
        Handles.Label(transform.position + (positionOffset), prefab.name);
    }
}
