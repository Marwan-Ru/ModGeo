using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * The octree class has been modified to contain vertices
 * in leaves
 */

public class Octree
{
    bool _isLeaf = false;
    private List<Octree> _childrens;
    (Vector3, Vector3) _boundingBox;
    private List<Vector3> _vertices = new List<Vector3>();

    public Octree((Vector3, Vector3) octreeRect, List<Vector3> vertices)
    {
        _boundingBox = octreeRect;
        _isLeaf = true;
        _vertices = vertices;
    }

    public Octree(List<Octree> childrens, (Vector3, Vector3) octreeRect)
    {
        _childrens = childrens;
    }

    public bool IsLeaf()
    {
        return _isLeaf;
    }

    public (Vector3, Vector3) GetBoundingBox()
    {
        return _boundingBox;
    }

    public Octree GetChild(int index)
    {
        return _childrens[index];
    }

    public List<Vector3> GetVertices()
    {
        return _vertices;
    }
}