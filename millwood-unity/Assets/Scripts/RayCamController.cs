using NUnit.Framework.Constraints;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class RayCamController : MonoBehaviour
{

    [Header("Entity \"RayCam\" parameters")]
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    private LayerMask layerMask;

    private ContactMesh contactMesh;

    [SerializeField] Transform contactMeshTransform;

    private void Awake()
    {
        layerMask = 1 << 8;
    }

    private void Start()
    {
        contactMesh = new();
    }
    void FixedUpdate()
    {

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, 10f, layerMask))
        {

            Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
            contactMesh.getObject().GetComponent<MeshRenderer>().material = renderer.material;

            contactMesh.updateTransform(hit.point, transform.rotation);

        } else {
            print("NoHit");
        }

    }

}

public class ContactMesh
{
    private Vector3[] vertices = new Vector3[4];
    private Vector2[] uv = new Vector2[4];
    private int[] triangles = new int[6];

    private GameObject meshObject;
    private Mesh mesh;

    public ContactMesh() {

        vertices[0] = new Vector3(-.2f, -.2f, 0);
        vertices[1] = new Vector3(-.2f, .2f, 0);
        vertices[2] = new Vector3(.2f, .2f, 0);
        vertices[3] = new Vector3(.2f, -.2f, 0);

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

        createMesh();
    }

    public void updateTransform(Vector3 transform, Quaternion rotation)
    {
        vertices[0] = transform + rotation * new Vector3(-.2f, -.2f, 0);
        vertices[1] = transform + rotation * new Vector3(-.2f, .2f, 0);
        vertices[2] = transform + rotation * new Vector3(.2f, .2f, 0);
        vertices[3] = transform + rotation * new Vector3(.2f, -.2f, 0);
        Object.Destroy(meshObject);
        createMesh();
    }

    public void createMesh()
    {
        mesh = new Mesh();
        mesh.name = "Custom mesh";

        meshObject = new GameObject("Mesh object", typeof(MeshRenderer), typeof(MeshFilter));

        meshObject.GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public GameObject getObject()
    {
        return meshObject;
    }
        
}
