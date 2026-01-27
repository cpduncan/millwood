using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class renderingtest1 : MonoBehaviour
{

    //Transform camCenter;

    //void Start()
    //{
    //    camCenter = GameObject.Find("center viewpoint").transform;
    //}

    [Header("Raycast Origin")]
    [SerializeField] public Transform rayOrigin;

    void FixedUpdate()
    {

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, fwd, out hit, 10))
        {
            print(hit.transform.gameObject);
        } else {
            print("NoHit");
        }

    }
}
