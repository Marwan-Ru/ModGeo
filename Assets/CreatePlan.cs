using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;


public class CreatePlan : MonoBehaviour
{
    public int height = 5;
    public int width = 3;

    // Start is called before the first frame update
    void Start()
    {
        
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        List<Vector3> vertices = new (); 
        List<int> triangles = new();
        int ny = this.height + 1; 
        int nx = this.width + 1;

        for (int i = 0; i < ny; i++)
        {
            for (int j = 0; j < nx; j ++)
            {
                vertices.Add(new Vector3(j, i, 0));
            }
        }


        for (int x = 0; x < ny - 1; x++)
        {
            for (int y = 0; y < nx - 1; y++)
            { 
                // Triangle number 1
                triangles.Add(x * nx + y); // top left
                triangles.Add((x + 1) * nx + y); // bottom left
                triangles.Add(x * nx + (y + 1)); // top right
                // Triangle number 2
                triangles.Add(x * nx + (y + 1)); // top right
                triangles.Add((x + 1) * nx + y); // bottom left
                triangles.Add((x + 1) * nx + (y + 1)); // bottom right
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
