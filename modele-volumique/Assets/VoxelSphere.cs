using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * Cette Classe Subdivise une sphère (continue) en voxels
 * Tout est paramétrable
 */

public enum OperatorType
{
    Intersection, Union, XOR
}

public class VoxelSphere : MonoBehaviour
{
    [SerializeField] private int depth = 3;
    [SerializeField] private bool adaptive = true;
    
    [SerializeField] private List<float> radiuses;
    [SerializeField] private List<Vector3> centers ;

    [SerializeField] private OperatorType op;
    
    private List<Octree> _octrees = new();

    private List<Octree> _leafs = new();
    
    private bool AABBIsOnSurface((Vector3, Vector3) boundingBox, int sphereindice)
    {
        var (pmin, pmax) = boundingBox;
        float dmin = 0;
        float dmax = 0;
        float r2 = radiuses[sphereindice] * radiuses[sphereindice];
        for(var i = 0; i < 3; i++ )
        {
            var a = Mathf.Pow(centers[sphereindice][i] - pmin[i], 2);
            var b = Mathf.Pow(centers[sphereindice][i] - pmax[i], 2);
            dmax += Mathf.Max(a, b);
            if (centers[sphereindice][i] < pmin[i])
            {
                dmin += a;
            }else if (centers[sphereindice][i] > pmax[i])
            {
                dmin += b;
            }
        }
        return (dmin <= r2 && dmax >= r2);
    }
    
    private Octree SubdivideTree((Vector3, Vector3) boundingBox, int sphereindice, int n = 0)
    {
        var (pmin, pmax) = boundingBox;
        Vector3 center = (pmin + pmax) * 0.5f;
        
        if (n < depth && (AABBIsOnSurface(boundingBox, sphereindice) || !adaptive)) // We subdivide
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
            
            List<Octree> childs = new();
            
            childs.Add(SubdivideTree( (pmin, center), sphereindice, n+1 ));
            childs.Add(SubdivideTree( (A, B), sphereindice, n+1 ));
            childs.Add(SubdivideTree( (C, D), sphereindice, n+1 ));
            childs.Add(SubdivideTree( (E, F), sphereindice, n+1 ));
            childs.Add(SubdivideTree( (K, L), sphereindice, n+1 ));
            childs.Add(SubdivideTree( (G, H), sphereindice, n+1 ));
            childs.Add(SubdivideTree( (I,J), sphereindice, n+1 ));
            childs.Add(SubdivideTree( (center, pmax), sphereindice, n+1 ));

            return new Octree(childs, boundingBox);
        }
        // Max depth or not on the edge (if adaptive) 
        
        bool isInside = (center - centers[sphereindice]).magnitude < radiuses[sphereindice];

        if (isInside && op == OperatorType.Union)
        {
           GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
           c.transform.position = center;
           c.transform.localScale *= (pmax - pmin).x;
        }
        
        return new Octree(isInside, boundingBox);
        
    }

    void GetLeafs(Octree octree)
    {

        if (octree.IsLeaf())
        {
            _leafs.Add(octree);
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                GetLeafs(octree.GetChild(i));
            }
        }
        
    }
    
    void IntersectionOctree(List<Octree> octrees)
    {
        foreach (var octree in octrees)
        {
            GetLeafs(octree);
        }

        
        foreach (var leaf in _leafs)
        {
            bool isInsideAll = true;

            var (pmin, pmax) = leaf.GetBoundingBox();
            Vector3 center = (pmin + pmax) * 0.5f;
            
            
            for (int i = 0; i < radiuses.Count; i++)
            {
                isInsideAll = isInsideAll && (center - centers[i]).magnitude < radiuses[i];
            }
            
            
            if (isInsideAll)
            {
                GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
                c.transform.position = center;
                c.transform.localScale *= (pmax - pmin).x;
            }
            
        }
        
    }
    
    void XorOctree(List<Octree> octrees)
    {
        foreach (var octree in octrees)
        {
            GetLeafs(octree);
        }

        
        foreach (var leaf in _leafs)
        {
            

            var (pmin, pmax) = leaf.GetBoundingBox();
            Vector3 center = (pmin + pmax) * 0.5f;
            
            bool isInsideAll = (center - centers[0]).magnitude < radiuses[0];
            
            for (int i = 1; i < radiuses.Count; i++)
            {
                isInsideAll = isInsideAll ^ (center - centers[i]).magnitude < radiuses[i];
            }
            
            
            if (isInsideAll)
            {
                GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
                c.transform.position = center;
                c.transform.localScale *= (pmax - pmin).x;
            }
            
        }
        
    }
    
    void Start()
    {
        for (int i = 0; i < radiuses.Count; i++)
        {
            Vector3 pmin = centers[i] - radiuses[i] * new Vector3(1, 1, 1);
            Vector3 pmax = centers[i] + radiuses[i] * new Vector3(1, 1, 1);

            _octrees.Add(SubdivideTree((pmin, pmax), i));
        }

        if (op == OperatorType.Intersection)
        {
            IntersectionOctree(_octrees);
        } else if (op == OperatorType.XOR)
        {
            XorOctree(_octrees);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
