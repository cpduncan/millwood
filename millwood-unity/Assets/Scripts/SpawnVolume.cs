using System;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class SpawnVolume : MonoBehaviour
{
    
    [SerializeField] private Vector3 halfExtents = new Vector3(5f, 5f, 5f);
    [SerializeField] private LayerMask layerMask;
    
    // Collider settings should "Include" / mask to player layer
    private void OnTriggerEnter(Collider other)
    {
        Collider[] hits = Physics.OverlapBox(transform.position, halfExtents, transform.rotation, layerMask);
        
        foreach (Collider hit in hits)
        {
            Spawner spawner = hit.GetComponent<Spawner>();
            if (spawner != null)
            {
                spawner.Spawn();
            }
        }
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position,
            transform.rotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);
    }
    
}
