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
    [SerializeField] private float stepSpeed;

    private bool stepping = false;
    private Vector3 goalPosition;
    
    private void Update()
    {
        if (!stepping)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, stepDepth, ~(1 << 8)))
            {
                goalPosition = hit.point;
            }
            if (Vector3.Distance(goalPosition, target.position) > stepDistance)
            {
                stepping = true;
            }
        }
        else
        {
            target.position = Vector3.Lerp(
                target.position,
                goalPosition,
                Time.deltaTime * stepSpeed
            );
            
            if (Vector3.Distance(goalPosition, target.position) < .1f)
            {
                stepping = false;
            }
        }
    }
}
