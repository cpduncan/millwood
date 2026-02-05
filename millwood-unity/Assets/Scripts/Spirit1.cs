using System;
using DitzelGames.FastIK;
using Unity.VisualScripting;
using UnityEngine;

public class Spirit1 : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    [Header("Enemy Parameters")]
    [SerializeField] private float speed;
    
    [Header("IK Parameters")]
    [SerializeField] private int jointNum;
    [SerializeField] private float boneLength;
    
    [Header("Head")]
    [SerializeField] private Transform head;
    private Quaternion originalHeadRotation;
    
    private GameObject[] _legs = new GameObject[4];
    private GameObject[] _targets = new GameObject[4];

    private void Awake()
    {
        originalHeadRotation = head.rotation;
        
        // inits top leg joints and foot targets to respective arrays (_legs and _targets)
        _legs[0] = NewLeg(0, new Vector3(-1, 0, -1));
        _legs[1] = NewLeg(1, new Vector3(-1, 0, 1));
        _legs[2] = NewLeg(2, new Vector3(1, 0, 1));
        _legs[3] = NewLeg(3, new Vector3(1, 0, -1));
    }

    private GameObject NewLeg(int index, Vector3 position)
    {
        GameObject[] joints = new GameObject[jointNum];
        
        joints[0] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        joints[0].name = "Joint 0";
        joints[0].transform.SetParent(transform, false); 
        joints[0].transform.localScale = new Vector3(.25f, .25f, .25f);
        joints[0].transform.position = transform.position + position;
        
        for (int i = 1; i < jointNum; i++) {
            joints[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            joints[i].name = "Joint " + i;
            joints[i].transform.SetParent(joints[i - 1].transform, false); 
            joints[i].transform.position += new Vector3(0, -boneLength, 0);
        }

        // ik setup per leg
        
        FastIKFabric script = joints[jointNum-1].AddComponent<FastIKFabric>();
        
        script.ChainLength = jointNum - 1;
        
        GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pole.name = "Pole " + index;
        pole.transform.SetParent(transform, false);
        pole.transform.localScale = new Vector3(.25f, .25f, .25f);
        pole.transform.position = transform.position + (position * (jointNum-1)) + (transform.up * boneLength);
        
        script.Pole = pole.transform;
        
        _targets[index] = new GameObject("Target " + index);
        script.Target = _targets[index].transform;
        
        // returns top leg joint
        return joints[0];
    }

    void Update()
    {
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        head.LookAt(playerTransform);
        head.transform.rotation *= originalHeadRotation;
        
        transform.position += new Vector3(transform.forward.x, 0, transform.forward.z).normalized * speed;
    }
}
