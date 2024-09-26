using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SphereConstructor : MonoBehaviour
{

    [SerializeField]
    private float radius = 1;

    [SerializeField]
    private int m = 3; // Le nombre de méridiens

    [SerializeField]
    private int p = 2; // Le nombre de paralleles

    // Start is called before the first frame update
    void Start()
    {
        // Minimum pour des 
        if (m < 3) m = 3;
        if (p < 2) p = 2;

        Vector3 N = new Vector3(0, 0, radius); // Pole nord
        Vector3 S = new Vector3(0, 0, - radius); // Pole sud

        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        List<Vector3> vertices = new();
        List<int> triangles = new();

        // Parrallèles 

        for (int j = 0; j < p; j++) // Paralleles
        {
            float phi_i =j * Mathf.PI / p;
            for (int i = 0; i < m; i++) // Meridiens sur chaque paralleles
            {
                float thau_i = 2 * Mathf.PI * i / m; // Angle

                vertices.Add(new Vector3(radius * Mathf.Cos(thau_i), radius - phi_i, radius * Mathf.Sin(thau_i)));
            }
        }

        // Adding south and north pole
        vertices.Add(N);
        vertices.Add(S);

        // Triangles

        for(int i = 0; i < p; i++)
        {
            for(int j = 0;j < m; j++)
            {
                triangles.Add(i * j);
                triangles.Add((i + 1) * j);
                triangles.Add(i * j + 1);

                triangles.Add(i * j);
                triangles.Add(i * j + 1);
                triangles.Add((i + 1) * j + 1);
            }
        }


        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
