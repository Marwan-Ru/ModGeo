using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable SuggestVarOrType_SimpleTypes

public class CellCollapse : MonoBehaviour
{
    [SerializeField] private int depth; // the higher this value, the higher the definition
    
    private Mesh _mesh;
    private Octree SubdivideTree((Vector3, Vector3) boundingBox, int n = 0)
    {
        var (pmin, pmax) = boundingBox;
        Vector3 center = (pmin + pmax) * 0.5f;
        
        if (n < depth) // We subdivide
        {
            // Creation des points nécessaires a la subdivision
            Vector3 A = new Vector3(center.x, pmin.y, pmin.z);
            Vector3 B = new Vector3(pmax.x, center.y, center.z);
            Vector3 C = new Vector3(pmin.x, center.y, pmin.z);
            Vector3 D = new Vector3(center.x, pmax.y, center.z);
            Vector3 E = new Vector3(center.x, center.y, pmin.z);
            Vector3 F = new Vector3(pmax.x, pmax.y, center.z);
            Vector3 G = new Vector3(center.x, pmin.y, center.z);
            Vector3 H = new Vector3(pmax.x, center.y, pmax.z);
            Vector3 I = new Vector3(pmin.x, center.y, center.z);
            Vector3 J = new Vector3(center.x, pmax.y, pmax.z);
            Vector3 K = new Vector3(pmin.x, pmin.y, center.z);
            Vector3 L = new Vector3(center.x, center.y, pmax.z);
            
            List<Octree> childrens = new()
            {
                SubdivideTree((pmin, center), n + 1),
                SubdivideTree((A, B), n + 1),
                SubdivideTree( (C, D), n+1 ),
                SubdivideTree( (E, F), n+1 ),
                SubdivideTree( (K, L), n+1 ),
                SubdivideTree( (G, H), n+1 ),
                SubdivideTree( (I,J), n+1 ),
                SubdivideTree( (center, pmax), n+1 )
            };

            return new Octree(childrens, boundingBox);
        }
        // Max depth we check for vertices inside aabb
        List<Vector3> vertices = new();
        
        foreach (var v in _mesh.vertices)
        {
            if (v.x > pmin.x && v.x <= pmax.x && v.y > pmin.y && v.y <= pmax.y && v.z > pmin.z && v.z <= pmax.z)
            {
                vertices.Add(v);
            }
        }
        
        return new Octree(boundingBox, vertices);
        
    }

    void cellCollapse(Octree octree)
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;

        Vector3 pmin = _mesh.vertices[0];
        Vector3 pmax = _mesh.vertices[1];
        
        
        foreach (var v in _mesh.vertices)
        {
            if (v.magnitude > pmax.magnitude)
            {
                pmax = v;
            }else if (v.magnitude < pmin.magnitude)
            {
                pmin = v;
            }
        }
        
        Octree spaceSubdivision = SubdivideTree((pmin, pmax));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
