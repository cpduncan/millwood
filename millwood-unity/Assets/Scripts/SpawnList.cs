using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class SpawnList : MonoBehaviour
{
    
    [SerializeField] private Vector3 halfExtents = new Vector3(5f, 5f, 5f);
    [SerializeField] private Spawner[] spawners;
    
    // Collider settings should "Include" / mask to player layer
    private void OnTriggerEnter(Collider other)
    {
        foreach (Spawner spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.Spawn();
            }
        }
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        if (spawners.Length == 0) return;
        
        Gizmos.color = Color.green;
        
        foreach (Spawner spawner in spawners)
        {
            Gizmos.DrawLine(
                transform.position,
                spawner.transform.position
            );
        }
    }
    
}
