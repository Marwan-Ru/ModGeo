using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderConstructor : MonoBehaviour
{
    /*We create a cylinder whose "axe" is on the z axis, we let unity do the rest
     The two planes at the top and bottom are at -h/2 and h/2, their normal is (0,0,-1) and (0,0,1) where (x,y,z)
     */

    [SerializeField]
    private float radius = 1;

    [SerializeField]
    private float height = 3;

    [SerializeField]
    private int m = 20; // Le nombre de méridiens

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        List<Vector3> vertices = new();
        List<int> triangles = new();


        for (int i = 0; i < m; i++) { 
            float thau_i = 2 * Mathf.PI * i / m; // Angle

            vertices.Add(new Vector3(radius * Mathf.Cos(thau_i), height / 2,  radius * Mathf.Sin(thau_i)));
            vertices.Add(new Vector3(radius * Mathf.Cos(thau_i), -(height / 2), radius * Mathf.Sin(thau_i)));
        }


        // Points at the center of each delimiting plane
        vertices.Add(new Vector3(0, height / 2, 0));
        vertices.Add(new Vector3(0, -(height / 2), 0));

        // Each side rectangle (for now)
        int pos = 0;

        for (int i = 0; i < (m * 2) - 2; i += 2) {
            triangles.Add(i);
            triangles.Add(i + 3);
            triangles.Add(i + 1);
          
            triangles.Add(i);
            triangles.Add(i + 2);
            triangles.Add(i + 3);

            pos = i + 2;
        }

        triangles.Add(pos);
        triangles.Add(1);
        triangles.Add(pos + 1);

        triangles.Add(pos);
        triangles.Add(0);
        triangles.Add(1);

        // Each plane
        for (int i = 0; i < m * 2; i+=2)
        {
            triangles.Add(i);
            triangles.Add(m * 2);
            triangles.Add(i + 2);
            
            triangles.Add(i + 1);
            triangles.Add(i + 3);
            triangles.Add((m * 2) + 1);
        }

        // Last triangles (Because we have to attach to the verw first vertices

        triangles.Add(pos);
        triangles.Add(m * 2);
        triangles.Add(0);

        triangles.Add(pos + 1);
        triangles.Add(1);
        triangles.Add((m * 2) + 1);


        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
