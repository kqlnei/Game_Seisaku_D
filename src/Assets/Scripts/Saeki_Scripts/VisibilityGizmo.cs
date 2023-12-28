using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityGizmo : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;
    private GameObject _Light_Gizmo;
    private bool Mat_Change;

    [SerializeField, Range(0, 20)] private float _sight_range_Y = 5;
    [SerializeField, Range(0, 360)] private float _sight_angle_Y = 120;
    [SerializeField] private Material Mat_Y;
    [SerializeField] private Material Mat_R;

    [SerializeField] GameObject Light;
    EnemyLight _LightTrigger;

    // Start is called before the first frame update
    void Start()
    {
        Mat_Change = false;

        _Light_Gizmo = Instantiate(new GameObject(), Vector3.down * 3f + Enemy.transform.position, Enemy.transform.rotation);
        _Light_Gizmo.transform.parent = Enemy.transform;
        _Light_Gizmo.AddComponent<MeshRenderer>();
        _Light_Gizmo.AddComponent<MeshFilter>();
        _Light_Gizmo.GetComponent<MeshRenderer>().material = Mat_Y;

        List <Vector3> vertices = new() { Vector3.zero };
        List<int> triangles = new();
       
        int i = 0;
        for (float d = -_sight_angle_Y / 2f; d <= _sight_angle_Y / 2f; d++)
        {
            float x = Mathf.Sin(d * Mathf.Deg2Rad) * _sight_range_Y;
            float y = Mathf.Cos(d * Mathf.Deg2Rad) * _sight_range_Y;

            vertices.Add(new Vector3(x, 0, y));
            triangles.AddRange(new int[] { 0, i + 1, i + 2 });

            i++;
        }

        triangles.RemoveRange(triangles.Count - 3, 3);

        Mesh mesh = new();
        
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        _Light_Gizmo.GetComponent<MeshFilter>().sharedMesh = mesh;

        _LightTrigger = Light.GetComponent<EnemyLight>();

        _Light_Gizmo.transform.localPosition = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        if (_LightTrigger.lightEnter && !Mat_Change)
        {
            _Light_Gizmo.GetComponent<MeshRenderer>().material = Mat_R;
            Mat_Change = true;
        }
            

        else if(!_LightTrigger.lightEnter && Mat_Change)
        {
            _Light_Gizmo.GetComponent<MeshRenderer>().material = Mat_Y;
            Mat_Change = false;
        }
    }
}
