using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class renderingtest1 : MonoBehaviour
{

    [SerializeField] Material mat;

    private Vector3[] verticies = new Vector3[4];
    private Vector2[] uv = new Vector2[4];
    private int[] triangles = new int[6];

    private GameObject meshObject;
    private Mesh mesh;

    //Transform camCenter;

    //void Start()
    //{
    //    camCenter = GameObject.Find("center viewpoint").transform;
    //}

    [Header("Raycast Origin")]
    [SerializeField] public Transform rayOrigin;

    private LayerMask layerMask;

    private void Awake()
    {
        layerMask = 1 << 8;
    }

    private void Start()
    {
        verticies[0] = new Vector3(0, 0, 0);
        verticies[1] = new Vector3(0, 1, 0);
        verticies[2] = new Vector3(1, 1, 0);
        verticies[3] = new Vector3(1, 0, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        mesh = new Mesh();
        mesh.name = "Custom mesh";

        meshObject = new GameObject("Mesh object", typeof(MeshRenderer), typeof(MeshFilter));

        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = mat;

        mesh.vertices = verticies;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    void FixedUpdate()
    {

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, fwd, out hit, 10f, layerMask))
        {
            Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
            meshObject.GetComponent<MeshRenderer>().material = renderer.material;
            //print(renderer.material.name);
        } else {
            print("NoHit");
        }

    }

}
