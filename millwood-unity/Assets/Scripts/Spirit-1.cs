using System;
using System.Linq;
using DitzelGames.FastIK;
using Unity.VisualScripting;
using UnityEngine;

public class Spirit1 : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float stepHeight;
    [SerializeField] private float stepInitiateDistance;
    [SerializeField] private float stepOvershotDistance;
    
    [SerializeField] private int jointNum;
    [SerializeField] private float boneLength;
    
    [SerializeField] private Transform body;
    [SerializeField] private float bodyHeight;
    
    private Quaternion originalBodyRotation;
    
    private GameObject[] _legs = new GameObject[4];
    private GameObject[] _targets = new GameObject[4];
    private Vector3[] _footIdeals = new Vector3[4];
    private bool[] _stepping = new bool[4]{false, false, false, false};
    private float[] _stepProgress = new float[4];

    void FixedUpdate()
    {
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        body.LookAt(playerTransform);
        body.transform.rotation *= originalBodyRotation;
        
        transform.position += speed * Time.deltaTime
                              * new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // Moving IK Targets to ideal step locations
        findFootIdeals();
        if (_stepping[0] || _stepping[1] || _stepping[2] || _stepping[3])
        { moveTargets(); }
        else
        { attemptInitiateSteps(); }

        // body positioned at avg step height + body height field value
        float averageStepHeight = 0;
        int groundedFootCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (!_stepping[i])
            {
                groundedFootCount++;
                averageStepHeight += _footIdeals[i].y;
            }
        }
        averageStepHeight /= groundedFootCount;
        transform.position = new Vector3(transform.position.x, bodyHeight + averageStepHeight, transform.position.z);

    }

    private void Awake()
    {
        originalBodyRotation = body.rotation;
        
        // inits top joints and foot targets to respective arrays: (_legs and _targets)
        _legs[0] = NewLeg(0, new Vector3(-1, 0, -1));
        _legs[1] = NewLeg(1, new Vector3(-1, 0, 1));
        _legs[2] = NewLeg(2, new Vector3(1, 0, 1));
        _legs[3] = NewLeg(3, new Vector3(1, 0, -1));
    }

    private void attemptInitiateSteps()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Vector3.Distance(_targets[i].transform.position, _footIdeals[i]) > stepInitiateDistance && 
                !(_stepping[0] || _stepping[1] || _stepping[2] || _stepping[3]))
            {
                _stepping[i] = true;
                _stepProgress[i] = 0f;
                _stepping[(i+2)%4] = true;
                _stepProgress[(i+2)%4] = 0f;
            }
        }
    }
    
    private void moveTargets()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_stepping[i])
            {
                _targets[i].transform.position = Vector3.MoveTowards(_targets[i].transform.position,
                    _footIdeals[i], speed * 3 * Time.deltaTime);
                
                // sin function for step height, the half-period is (stepInitiateDistance + stepOvershootDistance)
                
                //might be wrong 
                _stepProgress[i] += Time.deltaTime * speed * 3; 
                _stepProgress[i] = Mathf.Clamp01(_stepProgress[i]);
                
                // _targets[i].transform.position += new Vector3(0, Mathf.Sin(Mathf.PI * _stepProgress[i]) * stepHeight, 0);
                float height = Mathf.Sin(Mathf.PI * _stepProgress[i]) * stepHeight;
                
                _targets[i].transform.position += Vector3.up * height;
                    
                if (Vector3.Distance(_targets[i].transform.position, _footIdeals[i]) < .2f)
                { _stepping[i] = false; }
            }
        }
    }

    void findFootIdeals()
    {
        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(
                    _legs[i].transform.position,
                    transform.TransformDirection(Vector3.down),
                    out hit,
                    (jointNum-1) * boneLength, 
                    ~(8<<1)))
            { _footIdeals[i] = hit.point + (transform.forward * stepOvershotDistance); }
            else
            { _footIdeals[i] = _legs[i].transform.position + new Vector3(0, -jointNum * boneLength, 0); }
            
            // figure out a default position for legs
        }
        
    }

    private GameObject NewLeg(int index, Vector3 position)
    {
        GameObject[] joints = new GameObject[jointNum];
        
        joints[0] = new GameObject("Joint 0");
        joints[0].transform.SetParent(transform, false); 
        joints[0].transform.localScale = new Vector3(.25f, .25f, .25f);
        joints[0].transform.position = transform.position + position;
        
        for (int i = 1; i < jointNum; i++) {
            
            GameObject legPiece = GameObject.CreatePrimitive(PrimitiveType.Cube);
            legPiece.transform.SetParent(joints[i - 1].transform, false);
            legPiece.transform.position += new Vector3(0, -boneLength * .5f, 0);
            legPiece.transform.localScale += new Vector3(0, 4f, 0);
            legPiece.layer = 8;

            joints[i] = new GameObject("Joint " + i);
            joints[i].transform.SetParent(joints[i - 1].transform, false); 
            joints[i].transform.position += new Vector3(0, -boneLength, 0);
        }

        // ik setup per leg
        
        FastIKFabric script = joints[jointNum-1].AddComponent<FastIKFabric>();
        
        script.ChainLength = jointNum - 1;
        
        GameObject pole = new GameObject("Pole " + index);
        pole.transform.SetParent(transform, false);
        pole.transform.localScale = new Vector3(.25f, .25f, .25f);
        pole.transform.position = transform.position + (position * (jointNum-1)) + (transform.up * boneLength);
        
        script.Pole = pole.transform;
        
        _targets[index] = new GameObject("Target " + index);
        script.Target = _targets[index].transform;
        
        // returns top leg joint
        return joints[0];
    }

}
