using System;
using NUnit.Framework.Constraints;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class RayCamController : MonoBehaviour
{

    [Header("RayCam parameters")]
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    
    [SerializeField] private float meshSize;

    /*
     * vertical fov degree: halfCameraFOV
     * horizontal fov degree: arctan(tan(verticalFovDegree)*(16/9))
     */
    
    
    // [Range(-2.0f, 0f)] public float bottomLeftRayX;
    // private float bottomLeftRayX = -1.05f;
    // [Range(-2.0f, 0f)] public float bottomLeftRayY;
    // private float bottomLeftRayY = -0.6f;
    // [Range(0.0f, 1.0f)] public float rayPositionIncrement;

    private float _topFovAngle;
    private float _botLeftPosY;
    private float _rightFovAngle;
    private float _botLeftPosX;
    
    private ContactMesh[,] _meshes;
    private Vector3[,] _directions;

    private void Awake()
    {
        _topFovAngle = Camera.main.fieldOfView / 2f;
        _botLeftPosY = Mathf.Tan(_topFovAngle*Mathf.Deg2Rad);
        _rightFovAngle = Mathf.Atan(Mathf.Tan(_topFovAngle*Mathf.Deg2Rad) * (16f / 9f));
        _botLeftPosX = Mathf.Tan(_rightFovAngle);
        
        _meshes = new ContactMesh[gridWidth, gridHeight];
        _directions = new Vector3[gridWidth, gridHeight];

        float incriment = (_botLeftPosX*2)/gridWidth;
        
        float xEuler = -_botLeftPosX;
        float yEuler = -_botLeftPosY;
        float yEulerReset = yEuler;
        for (int col = 0; col < gridWidth; col++)
        {
            for (int row = 0; row < gridHeight; row++)
            {
                _meshes[col, row] = new ContactMesh(meshSize); 
                _directions[col, row] = new Vector3(xEuler, yEuler, 1);
                    
                yEuler += incriment;
            }
            xEuler += incriment;
            yEuler = yEulerReset;
        }
    }

    void FixedUpdate()
    {
        for (int col = 0; col < gridWidth; col++)
        {
            for (int row = 0; row < gridHeight; row++)
            {
                _meshes[col, row].Cast(transform, transform.rotation*_directions[col, row]);
            }
        }
    }
}

public class ContactMesh
{
    private readonly Vector3[] _vertices = new Vector3[4];
    private readonly Vector2[] _uv = new Vector2[4];
    private readonly int[] _triangles = new int[6];

    private readonly GameObject _meshObject;
    private readonly MeshRenderer _meshRenderer;

    public void Cast(Transform transform, Vector3 rotation)
    {
        if (Physics.Raycast(transform.position, rotation, out RaycastHit hit, 20f, 1 << 8))
        {
            _meshObject.transform.position = hit.point;
            _meshObject.transform.rotation = transform.rotation * Quaternion.Euler(rotation);

            Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
            _meshRenderer.material = renderer.material;

        } else {
            _meshObject.transform.position = transform.position;
            _meshObject.transform.rotation = transform.rotation * Quaternion.Euler(rotation);
        }
    }

    public ContactMesh(float meshSize) {
        
        float halfMeshSize = meshSize / 2f;

        _vertices[0] = new Vector3(-halfMeshSize, -halfMeshSize, 0);
        _vertices[1] = new Vector3(-halfMeshSize, halfMeshSize, 0);
        _vertices[2] = new Vector3(halfMeshSize, halfMeshSize, 0);
        _vertices[3] = new Vector3(halfMeshSize, -halfMeshSize, 0);

        _triangles[0] = 0;
        _triangles[1] = 1;
        _triangles[2] = 2;

        _triangles[3] = 0;
        _triangles[4] = 2;
        _triangles[5] = 3;

        _uv[0] = new Vector2(0, 0);
        _uv[1] = new Vector2(0, 1);
        _uv[2] = new Vector2(1, 1);
        _uv[3] = new Vector2(1, 0);

        Mesh _mesh = new Mesh();
        _mesh.name = "Custom mesh";

        _meshObject = new GameObject("Mesh object", typeof(MeshRenderer), typeof(MeshFilter));

        _meshObject.GetComponent<MeshFilter>().mesh = _mesh;
        
        _meshRenderer = _meshObject.GetComponent<MeshRenderer>();

        _mesh.vertices = _vertices;
        _mesh.uv = _uv;
        _mesh.triangles = _triangles;
    }

}
