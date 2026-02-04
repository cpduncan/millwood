using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class IKController : MonoBehaviour
{
    [Header("IK Step Parameters")]
    [SerializeField] private Transform target;
    [SerializeField] private float stepDepth;
    [SerializeField] private float stepDistance;
    
    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, stepDepth, ~(1 << 8)))
        {
            target.position = Vector3.Lerp(
                target.position,
                hit.point,
                Time.deltaTime * 10f
            );
        }
    }
}
