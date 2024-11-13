using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
// ReSharper disable SuggestVarOrType_SimpleTypes

public class CellCollapse : MonoBehaviour
{
    [SerializeField] private int depth; // the higher this value, the higher the definition
    
    private Mesh _mesh;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    
    private List<Vector3> _newVertices = new();
    private List<int> _newTriangles = new();
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
        List<int> verticesIndices = new();
        
        for (int i=0; i < _vertices.Count; i++)
        {
            var v = _vertices[i];
            if (v.x > pmin.x && v.x <= pmax.x && v.y > pmin.y && v.y <= pmax.y && v.z > pmin.z && v.z <= pmax.z)
            {
                verticesIndices.Add(i);
            }
        }
        
        return new Octree(boundingBox, verticesIndices);
        
    }

    void Collapse(Octree octree)
    {
        if (!octree.IsLeaf())
        {
            for (int i = 0; i < 8; i++)
            {
                Collapse(octree.GetChild(i));
            }
        }
        
        if (octree.GetVerticesIndices().Count > 0)
        {
            Vector3 mean = Vector3.zero;
            foreach (var v in octree.GetVerticesIndices())
            {
                mean += _vertices[v];
            }
            mean /= octree.GetVerticesIndices().Count;
            _newVertices.Add(mean);
            octree.SetRepresentative(_newVertices.Count - 1);
        }
        
    }
    
    void BuildTriangles(Octree octree)
    {
        if (!octree.IsLeaf())
        {
            for (int i = 0; i < 8; i++)
            {
                BuildTriangles(octree.GetChild(i));
            }
        }
        else
        {
            if (octree.GetVerticesIndices().Count > 0 && octree.HasRepresentative())
            {
                foreach (var t in octree.GetVerticesIndices())
                {
                    for (int i = 0; i < _triangles.Count; i++)
                    {
                        if (_triangles[i] == t)
                        {
                            _triangles[i] = octree.GetRepresentativeIndice() + 1000000000;
                        }
                    }
                }
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _vertices = _mesh.vertices.ToList();
        _triangles = _mesh.triangles.ToList();

        Vector3 pmin = _vertices[0];
        Vector3 pmax = _vertices[1];


        foreach (var v in _vertices)
        {
            pmax = Vector3.Max(v, pmax);
            pmin = Vector3.Min(v, pmin);
        }

        Octree subdivision = SubdivideTree((pmin, pmax));
        Collapse(subdivision);
        BuildTriangles(subdivision);

        // On remet aux vrais indices
        for (int i = 0; i < _triangles.Count; i++)
        {
            if (_triangles[i] < 0) print("????");
            _triangles[i] -= 1000000000;
            if (_triangles[i] < 0) print("bizarre");
        }
        
        foreach (var t in _triangles)
        {
            if (t >= _newVertices.Count || t < 0) print(t);
        }
        
        _mesh.Clear();
        
        _mesh.vertices = _newVertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        
        _mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
