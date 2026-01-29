using NUnit.Framework.Constraints;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class RayCamController : MonoBehaviour
{

    [Header("Entity \"RayCam\" parameters")]
    [SerializeField] private int gridWidth = 16;
    [SerializeField] private int gridHeight = 9;

    private ContactMesh[,] meshes;
    private Vector3[,] directions;

    private void Awake()
    {
        meshes = new ContactMesh[gridWidth, gridHeight];
        directions = new Vector3[gridWidth, gridHeight];

        float incriment = 0.1f;

        //float xEuler = (gridWidth*incriment)/2;
        float xEuler = -0.8f;
        //float yEuler = (gridHeight*incriment)/2;
        float yEuler = -0.45f;
        for (int col = 0; col < gridWidth; col++)
        {
            for (int row = 0; row < gridHeight; row++)
            {
                meshes[col, row] = new();
                directions[col, row] = new Vector3(xEuler, yEuler, 1);
                    
                yEuler += incriment;
            }
            xEuler += incriment;
            //yEuler = (gridHeight*incriment)/2;
            yEuler = -0.45f;
        }
    }

    void FixedUpdate()
    {
        for (int col = 0; col < gridWidth; col++)
        {
            for (int row = 0; row < gridHeight; row++)
            {
                meshes[col, row].cast(transform, transform.rotation*directions[col, row]);
            }
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

    public void cast(Transform transform, Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rotation, out hit, 20f, 1 << 8))
        {
            meshObject.transform.position = hit.point;
            meshObject.transform.rotation = transform.rotation * Quaternion.Euler(rotation);

            Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
            meshObject.GetComponent<MeshRenderer>().material = renderer.material;

        } else {
            //print("NoHit");
        }
    }

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

        mesh = new Mesh();
        mesh.name = "Custom mesh";

        meshObject = new GameObject("Mesh object", typeof(MeshRenderer), typeof(MeshFilter));

        meshObject.GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}
