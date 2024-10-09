using System.Collections.Generic;
using UnityEngine;

public class SphereConstructor : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;

    [SerializeField]
    private int m = 24; // Number of meridians (longitude lines)

    [SerializeField]
    private int p = 12; // Number of parallels (latitude lines)

    void Start()
    {
        if (m < 3) m = 3;
        if (p < 2) p = 2;

        Vector3 N = new Vector3(0, 0, radius); // North pole
        Vector3 S = new Vector3(0, 0, -radius); // South pole

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        List<Vector3> vertices = new();
        List<int> triangles = new();

        // Calculate vertices
        for (int j = 0; j <= p; j++) // Parallels
        {
            float phi = Mathf.PI * j / p; // Latitude angle

            for (int i = 0; i < m; i++) // Meridians
            {
                float theta = 2 * Mathf.PI * i / m; // Longitude angle
                float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = radius * Mathf.Cos(phi);
                float z = radius * Mathf.Sin(phi) * Mathf.Sin(theta);

                vertices.Add(new Vector3(x, y, z));
            }
        }

        // Add north and south poles
        vertices.Add(N);
        vertices.Add(S);

        // Calculate triangles
        for (int j = 0; j < p; j++)
        {
            for (int i = 0; i < m; i++)
            {
                int current = j * m + i;
                int next = (i + 1) % m + j * m;

                triangles.Add(current);
                triangles.Add(next);
                triangles.Add(current + m);

                triangles.Add(next);
                triangles.Add(next + m);
                triangles.Add(current + m);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        // mesh.RecalculateNormals(); // Recalculate normals for lighting
    }

    void Update()
    {
        // Any updates or interactions can be handled here
    }
}
