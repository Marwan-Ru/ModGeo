using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * Cette Classe Subdivise une sphère (continue) en voxels
 * Tout est paramétrable
 */

public class VoxelSphere : MonoBehaviour
{
    [SerializeField] private int depth = 3;
    [SerializeField] private bool adaptive = true;
    
    [SerializeField] private List<float> radiuses;
    [SerializeField] private List<Vector3> centers ;

    private bool AABBIsOnSurface((Vector3, Vector3) boundingBox)
    {
        var (pmin, pmax) = boundingBox;
        float dmin = 0;
        float dmax = 0;
        float r2 = radiuses[0] * radiuses[0];
        for(var i = 0; i < 3; i++ )
        {
            var a = Mathf.Pow(centers[0][i] - pmin[i], 2);
            var b = Mathf.Pow(centers[0][i] - pmax[i], 2);
            dmax += Mathf.Max(a, b);
            if (centers[0][i] < pmin[i])
            {
                dmin += a;
            }else if (centers[0][i] > pmax[i])
            {
                dmin += b;
            }
        }
        return (dmin <= r2 && dmax >= r2);
    }
    
    private Octree SubdivideTree((Vector3, Vector3) boundingBox, int n = 0)
    {
        var (pmin, pmax) = boundingBox;
        Vector3 center = (pmin + pmax) * 0.5f;
        
        if (n < depth && (AABBIsOnSurface(boundingBox) || !adaptive)) // We subdivide
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
            
            childs.Add(SubdivideTree( (pmin: pmin, center), n+1 ));
            childs.Add(SubdivideTree( (A, B), n+1 ));
            childs.Add(SubdivideTree( (C, D), n+1 ));
            childs.Add(SubdivideTree( (E, F), n+1 ));
            childs.Add(SubdivideTree( (K, L), n+1 ));
            childs.Add(SubdivideTree( (G, H), n+1 ));
            childs.Add(SubdivideTree( (I,J), n+1 ));
            childs.Add(SubdivideTree( (center, pmax: pmax), n+1 ));

            return new Octree(childs, boundingBox);
        }
        // Max depth or not on the edge (if adaptive) 
        
        bool isInside = (center - centers[0]).magnitude < radiuses[0];

        if (isInside)
        {
           GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
           c.transform.position = center;
           c.transform.localScale *= (pmax - pmin).x;
        }
        
        return new Octree(isInside, boundingBox);
        
    }
    
    void Start()
    {
        centers.Add(transform.position);
        radiuses.Add(5);
        
        Vector3 pmin = centers[0] - radiuses[0] * new Vector3(1, 1, 1);
        Vector3 pmax = centers[0] + radiuses[0] * new Vector3(1, 1, 1);
        
        Octree sphereOctree = SubdivideTree((pmin, pmax));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
